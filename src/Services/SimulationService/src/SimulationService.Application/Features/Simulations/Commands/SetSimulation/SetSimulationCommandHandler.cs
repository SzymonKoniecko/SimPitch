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
using SimulationService.Domain.Entities;

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
        // create new simulation ID
        var simulationId = Guid.NewGuid();

        // create initial state
        var state = new SimulationState(simulationId, 0, progress: 0.00f, SimulationStatus.Pending, DateTime.Now);

        // save initial state in Redis
        await _registry.SetStateAsync(simulationId, state, cancellationToken);

        // create job payload
        var job = new SimulationJob(
            simulationId,
            SimulationParamsMapper.ToValueObject(command.SimulationParamsDto),
            state
        );

        // enqueue the job for background processing
        await _queue.EnqueueAsync(job, cancellationToken);

        // return the unique simulation identifier
        return simulationId;
    }
}