using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;

public class GetAllSimulationOverviewsQueryHandler : IRequestHandler<GetAllSimulationOverviewsQuery, List<SimulationOverviewDto>>
{
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public GetAllSimulationOverviewsQueryHandler(ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }

    public async Task<List<SimulationOverviewDto>> Handle(GetAllSimulationOverviewsQuery query, CancellationToken cancellationToken)
    {
        var result = await _simulationEngineGrpcClient.GetSimulationOverviewsAsync(cancellationToken);

        return result;
    }
}
