using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

public class GetSimulationResultsBySimulationIdQueryHandler : IRequestHandler<GetSimulationResultsBySimulationIdQuery, List<SimulationResultDto>>
{
    private readonly ISimulationResultGrpcClient _simulationResultGrpcClient;

    public GetSimulationResultsBySimulationIdQueryHandler(ISimulationResultGrpcClient simulationResultGrpcClient)
    {
        _simulationResultGrpcClient = simulationResultGrpcClient;
    }

    public async Task<List<SimulationResultDto>> Handle(GetSimulationResultsBySimulationIdQuery request, CancellationToken cancellationToken)
    {
        var simulationResults = await _simulationResultGrpcClient.GetSimulationResultsBySimulationIdAsync(request.SimulationId, cancellationToken);
        return simulationResults;
    }
}
