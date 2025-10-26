using System;
using MediatR;
using Microsoft.Extensions.Logging;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Application.Features.Simulations.Commands.StopSimulation;

public class StopSimulationCommandHandler : IRequestHandler<StopSimulationCommand, string>
{
    private readonly IRedisSimulationRegistry _registry;
    private readonly ISimulationStateReadRepository _simulationStateReadRepository;
    private readonly ISimulationStateWriteRepository _simulationStateWriteRepository;
    private readonly ILogger<StopSimulationCommandHandler> _logger;

    public StopSimulationCommandHandler(
        IRedisSimulationRegistry registry,
        ISimulationStateReadRepository simulationStateReadRepository,
        ISimulationStateWriteRepository simulationStateWriteRepository,
        ILogger<StopSimulationCommandHandler> logger)
    {
        _registry = registry;
        _simulationStateReadRepository = simulationStateReadRepository;
        _simulationStateWriteRepository = simulationStateWriteRepository;
        _logger = logger;
    }

    public async Task<string> Handle(StopSimulationCommand request, CancellationToken cancellationToken)
    {
        var redisState = await _registry.GetStateAsync(request.SimulationId, cancellationToken);

        if (redisState is null)
        {
            _logger.LogWarning("Simulation {SimulationId} not found in Redis.", request.SimulationId);
            throw new KeyNotFoundException($"Simulation {request.SimulationId} not found.");
        }

        if (redisState.State is SimulationStatus.Cancelled 
            or SimulationStatus.Completed 
            or SimulationStatus.Failed)
        {
            _logger.LogInformation("Simulation {SimulationId} already in terminal state: {State}", 
                request.SimulationId, redisState.State);
            return redisState.State.ToString();
        }

        var cancelledState = redisState.SetCancelled();
        await _registry.SetStateAsync(request.SimulationId, cancelledState, cancellationToken);
        _logger.LogInformation("Simulation {SimulationId} marked as Cancelled in Redis.", request.SimulationId);

        try
        {
            var dbState = await _simulationStateReadRepository.GetSimulationStateByIdAsync(request.SimulationId, cancellationToken);

            if (dbState is null)
            {
                _logger.LogWarning("No SQL record found for Simulation {SimulationId}. Creating new Cancelled record.", request.SimulationId);
                dbState = new SimulationState
                {
                    SimulationId = request.SimulationId,
                    ProgressPercent = cancelledState.ProgressPercent,
                    LastCompletedIteration = cancelledState.LastCompletedIteration,
                    State = SimulationStatus.Cancelled,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            else
            {
                dbState.State = SimulationStatus.Cancelled;
                dbState.UpdatedAt = DateTime.UtcNow;
            }

            await _simulationStateWriteRepository.UpdateOrCreateAsync(dbState, cancellationToken);
            _logger.LogInformation("Simulation {SimulationId} marked as Cancelled in SQL.", request.SimulationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update SQL state for Simulation {SimulationId}", request.SimulationId);
        }

        return _simulationStateReadRepository.GetSimulationStateByIdAsync(request.SimulationId, cancellationToken).Result.State.ToString();
    }
}