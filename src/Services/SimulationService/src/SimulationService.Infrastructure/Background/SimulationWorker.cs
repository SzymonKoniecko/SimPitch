using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.Consts;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Background;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;

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

                    // run simulation in background
                    _ = ProcessSimulationAsync(job, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break; // graceful shutdown
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
            var stateRepo = scope.ServiceProvider.GetRequiredService<ISimulationStateWriteRepository>();
            var stateReadRepo = scope.ServiceProvider.GetRequiredService<ISimulationStateReadRepository>();

            try
            {
                await registry.SetStateAsync(job.SimulationId, job.State, stoppingToken);

                var overview = new SimulationOverview
                {
                    Id = job.SimulationId,
                    Title = $"Title: {DateTime.UtcNow:HH:mm:ss}",
                    CreatedDate = DateTime.UtcNow,
                    SimulationParams = JsonConvert.SerializeObject(job.Params)
                };
                await overviewRepo.CreateSimulationOverviewAsync(overview, stoppingToken);

                var cmd = new RunSimulationCommand(overview, job.SimulationId, SimulationParamsMapper.ToDto(job.Params), job.State);

                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                var watcher = Task.Run(async () =>
                {
                    while (!linkedCts.IsCancellationRequested)
                    {
                        var currentState = await registry.GetStateAsync(job.SimulationId);
                        if (currentState?.State == SimulationStatus.Cancelled)
                            linkedCts.Cancel();
                        await Task.Delay(SimulationConsts.ITERATION_DELAY, linkedCts.Token);
                    }
                }, linkedCts.Token);

                await mediator.Send(cmd, linkedCts.Token);

                if (await stateReadRepo.IsSimulationStateCancelled(job.SimulationId, stoppingToken))
                {
                    _logger.LogInformation("Simulation {SimulationId} has been cancelled.", job.SimulationId);
                    job.State.SetCancelled();
                }
                else
                {
                    var completedState = job.State.SetCompleted();
                    await registry.SetStateAsync(job.SimulationId, completedState, stoppingToken);

                    await stateRepo.ChangeStatusAsync(job.SimulationId, SimulationStatus.Completed, stoppingToken);
                    _logger.LogInformation("Simulation {SimulationId} completed successfully.", job.SimulationId);
                }
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException ||
                    ex.InnerException is OperationCanceledException ||
                    (ex is InvalidOperationException && ex.Message.Contains("Operation cancelled by user", StringComparison.OrdinalIgnoreCase)))
                {
                    await registry.SetStateAsync(job.SimulationId, job.State.SetCancelled(), stoppingToken);
                    _logger.LogInformation("Simulation {SimulationId} cancelled by user (SQL cancellation detected)", job.SimulationId);
                    return;
                }
                else
                {
                    _logger.LogError(ex, "Simulation {SimulationId} failed", job.SimulationId);
                    await registry.SetStateAsync(job.SimulationId, job.State.SetFailed(), stoppingToken);
                }

            }
            finally
            {
                await stateRepo.ChangeStatusAsync(job.SimulationId, job.State.State, stoppingToken);
            }
        }
    }
}
