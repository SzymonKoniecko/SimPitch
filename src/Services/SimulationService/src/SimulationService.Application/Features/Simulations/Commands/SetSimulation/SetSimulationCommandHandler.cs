using System;
using MediatR;
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

namespace SimulationService.Application.Features.Simulations.Commands.SetSimulation;

public class SetSimulationCommandHandler : IRequestHandler<SetSimulationCommand, Guid>
{
    private readonly ISimulationQueue _queue;
    private readonly IRedisSimulationRegistry _registry;

    public SetSimulationCommandHandler(ISimulationQueue queue, IRedisSimulationRegistry registry)
    {
        _queue = queue;
        _registry = registry;
    }

    public async Task<Guid> Handle(SetSimulationCommand command, CancellationToken cancellationToken)
    {
        Guid simulationId = Guid.NewGuid();

        var state = new SimulationState(SimulationStatus.Pending, 0, DateTime.UtcNow);
        await _registry.SetStateAsync(simulationId, state, cancellationToken);

        // enqueue background job
        _queue.Enqueue(
            new SimulationJob(
                simulationId,
                SimulationParamsMapper.ToValueObject(command.SimulationParamsDto),
                state)
            );

        return simulationId;
    }
}