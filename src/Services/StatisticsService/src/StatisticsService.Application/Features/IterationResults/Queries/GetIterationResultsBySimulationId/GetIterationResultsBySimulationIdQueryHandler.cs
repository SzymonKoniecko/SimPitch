using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, List<IterationResultDto>>
{
    private readonly IIterationResultGrpcClient _IterationResultGrpcClient;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultGrpcClient IterationResultGrpcClient)
    {
        _IterationResultGrpcClient = IterationResultGrpcClient;
    }

    public async Task<List<IterationResultDto>> Handle(GetIterationResultsBySimulationIdQuery request, CancellationToken cancellationToken)
    {
        var IterationResults = await _IterationResultGrpcClient.GetAllIterationResultsBySimulationIdAsync(request.SimulationId, cancellationToken: cancellationToken);
        return IterationResults;
    }
}
