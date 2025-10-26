using System;
using MediatR;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationStateById;

public class GetSimulationStateByIdQueryHandler : IRequestHandler<GetSimulationStateByIdQuery, SimulationState>
{
    private readonly IRedisSimulationRegistry _redisSimulationRegistry;

    public GetSimulationStateByIdQueryHandler(IRedisSimulationRegistry redisSimulationRegistry)
    {
        _redisSimulationRegistry = redisSimulationRegistry;
    }

    public async Task<SimulationState> Handle(GetSimulationStateByIdQuery query, CancellationToken cancellationToken)
    {
        return await _redisSimulationRegistry.GetStateAsync(query.simulationId);
    }
}
