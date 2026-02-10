using System;
using MediatR;
using Microsoft.Extensions.Logging;
using SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;
using SimulationService.Application.Features.Scoreboards.Commands;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.Interfaces.Write;

namespace SimulationService.Application.Features.Predict.Commands.SyncPredictionIterationResultCommand;

public class SyncPredictionIterationResultCommandHandler : IRequestHandler<SyncPredictionIterationResultCommand, bool>
{
    private readonly IMediator _mediator;
    private readonly IRedisSimulationRegistry _registry;
    private readonly ILogger<SyncPredictionIterationResultCommandHandler> _logger;
    private readonly ISimulationStateReadRepository _simulationStateReadRepository;
    private readonly ISimulationStateWriteRepository _simulationStateWriteRepository;

    public SyncPredictionIterationResultCommandHandler(
        IMediator mediator,
        IRedisSimulationRegistry registry,
        ILogger<SyncPredictionIterationResultCommandHandler> logger,
        ISimulationStateReadRepository simulationStateReadRepository,
        ISimulationStateWriteRepository simulationStateWriteRepository)
    {
        _mediator = mediator;
        _registry = registry;
        _logger = logger;
        _simulationStateReadRepository = simulationStateReadRepository;
        _simulationStateWriteRepository = simulationStateWriteRepository;
    }

    public async Task<bool> Handle(SyncPredictionIterationResultCommand command, CancellationToken cancellationToken)
    {

        var commandIterationResult = new CreateIterationResultCommand(command.IterationResult);
        await _mediator.Send(commandIterationResult, cancellationToken: cancellationToken);

        var state = await _simulationStateReadRepository.GetSimulationStateBySimulationIdAsync(command.IterationResult.SimulationId, cancellationToken);
        var overview = await _mediator.Send(new GetSimulationOverviewByIdQuery(command.IterationResult.SimulationId));
        state.LastCompletedIteration += 1; // adding one more, because of executed: commandIterationResult
        await UpdateStates(command.IterationResult.SimulationId, overview.SimulationParams.Iterations, state, cancellationToken);

        if (overview.SimulationParams.CreateScoreboardOnCompleteIteration)
        {
            if (await _mediator.Send(new CreateScoreboardByIterationResultCommand(overview, command.IterationResult), cancellationToken) == false)
                _logger.LogError($"Scoreboard is not created-> IterationResultId:{command.IterationResult.Id}, SimulationId: {command.IterationResult.SimulationId}");
        }
        return true;
    }

    private async Task UpdateStates(Guid simulationId, int maxIterations, SimulationState state, CancellationToken cancellationToken)
    {
        if (state.LastCompletedIteration == maxIterations)
        {
            var completedState = state.SetCompleted();
            await _registry.SetStateAsync(simulationId, completedState, cancellationToken);
            await _simulationStateWriteRepository.ChangeStatusAsync(simulationId, SimulationStatus.Completed, cancellationToken);
            _logger.LogInformation("Simulation {SimulationId} completed successfully. [Finished by SyncPredictionIterationResultCommandHandler]", simulationId);
            
            return;
        }

        await _registry.SetStateAsync(simulationId, state.Update((float)state.LastCompletedIteration / maxIterations * 100));
        await _simulationStateWriteRepository.UpdateOrCreateAsync(state, cancellationToken: cancellationToken);
    }
}
