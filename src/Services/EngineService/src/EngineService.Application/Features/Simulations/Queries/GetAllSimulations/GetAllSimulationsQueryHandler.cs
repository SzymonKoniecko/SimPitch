using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulations;

public class GetAllSimulationsQueryHandler : IRequestHandler<GetAllSimulationsQuery, List<SimulationOverviewDto>>
{
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public GetAllSimulationsQueryHandler(ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }

    public async Task<List<SimulationOverviewDto>> Handle(GetAllSimulationsQuery query, CancellationToken cancellationToken)
    {
        var result = await _simulationEngineGrpcClient.GetSimulationOverviewAsync(cancellationToken);

        return result;
    }
}
