using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Background;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Infrastructure.Background
{
    public class SimulationWorker : BackgroundService
    {
        private readonly ILogger<SimulationWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISimulationQueue _queue;

        public SimulationWorker(
            ILogger<SimulationWorker> logger,
            IServiceScopeFactory scopeFactory,
            ISimulationQueue queue)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SimulationWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var job = await _queue.DequeueAsync(stoppingToken);
                    if (job is null)
                    {
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }

                    // process simulation in background
                    _ = ProcessSimulationAsync(job, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // graceful shutdown
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in SimulationWorker loop.");
                    await Task.Delay(2000, stoppingToken);
                }
            }

            _logger.LogInformation("SimulationWorker stopped");
        }

        private async Task ProcessSimulationAsync(SimulationJob job, CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var registry = scope.ServiceProvider.GetRequiredService<IRedisSimulationRegistry>();
            var overviewRepo = scope.ServiceProvider.GetRequiredService<ISimulationOverviewWriteRepository>();

            try
            {
                var state = new SimulationState(SimulationStatus.Running, 0, DateTime.UtcNow);
                await registry.SetStateAsync(job.SimulationId, state, stoppingToken);

                var overview = new SimulationOverview
                {
                    Id = job.SimulationId,
                    Title = $"Title: {DateTime.UtcNow.TimeOfDay}",
                    CreatedDate = DateTime.UtcNow,
                    SimulationParams = JsonConvert.SerializeObject(job.Params)
                };
                await overviewRepo.CreateSimulationOverviewAsync(overview, stoppingToken);

                var cmd = new RunSimulationCommand(job.SimulationId, SimulationParamsMapper.ToDto(job.Params), state);

                // Forward stop signals from Redis
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                var watcher = Task.Run(async () =>
                {
                    while (!linkedCts.IsCancellationRequested)
                    {
                        var currentState = await registry.GetStateAsync(job.SimulationId);
                        if (currentState?.SimulationStatus == SimulationStatus.Cancelled)
                            linkedCts.Cancel(); // stop simulation
                        await Task.Delay(500, linkedCts.Token);
                    }
                }, linkedCts.Token);

                await mediator.Send(cmd, linkedCts.Token);

                await registry.SetStateAsync(job.SimulationId, state.SetCompleted(), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                await registry.SetStateAsync(job.SimulationId,
                    new SimulationState(SimulationStatus.Cancelled, 0, DateTime.UtcNow));
                _logger.LogInformation("Simulation {SimulationId} stopped by user", job.SimulationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Simulation {SimulationId} failed", job.SimulationId);
                await registry.SetStateAsync(job.SimulationId,
                    new SimulationState(SimulationStatus.Failed, 0, DateTime.UtcNow));
            }
        }
    }
}