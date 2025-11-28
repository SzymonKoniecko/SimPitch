using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetSimulationOverviewBySimulationId;

public class GetSimulationOverviewBySimulationIdQueryHandler : IRequestHandler<GetSimulationOverviewBySimulationIdQuery, SimulationOverviewDto>
{
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public GetSimulationOverviewBySimulationIdQueryHandler(ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }
    
    public async Task<SimulationOverviewDto> Handle(GetSimulationOverviewBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        return await _simulationEngineGrpcClient.GetSimulationOverviewBySimulationId(query.SimulationId, cancellationToken);
    }
}
