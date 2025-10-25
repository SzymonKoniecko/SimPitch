using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Infrastructure.Background;

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
                if (_queue.TryDequeue(out var job))
                {
                    using var scope = _scopeFactory.CreateScope();

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var registry = scope.ServiceProvider.GetRequiredService<IRedisSimulationRegistry>();
                    var overviewRepo = scope.ServiceProvider.GetRequiredService<ISimulationOverviewWriteRepository>();

                    var state = new SimulationState(SimulationStatus.Running, 0, DateTime.Now);
                    await registry.SetStateAsync(job.SimulationId, state, stoppingToken);

                    var overview = new SimulationOverview
                    {
                        Id = job.SimulationId,
                        Title = $"Title: {DateTime.UtcNow.TimeOfDay}",
                        CreatedDate = DateTime.UtcNow,
                        SimulationParams = JsonConvert.SerializeObject(job.Params)
                    };
                    await overviewRepo.CreateSimulationOverviewAsync(overview, stoppingToken);

                    var cmd = new RunSimulationCommand(
                        job.SimulationId,
                        SimulationParamsMapper.ToDto(job.Params),
                        state);
                        
                    await mediator.Send(cmd, stoppingToken);

                    await registry.SetStateAsync(job.SimulationId, state.SetCompleted(), stoppingToken);
                }
                else
                {
                    await Task.Delay(1000, stoppingToken); // idle wait
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing simulation job.");
            }
        }

        _logger.LogInformation("SimulationWorker stopped");
    }
}