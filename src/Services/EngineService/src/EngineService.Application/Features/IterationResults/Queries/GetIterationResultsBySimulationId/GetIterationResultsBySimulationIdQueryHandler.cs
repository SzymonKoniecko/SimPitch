using System;
using MediatR;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, List<IterationResultDto>>
{
    private readonly IIterationResultGrpcClient _IterationResultGrpcClient;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultGrpcClient IterationResultGrpcClient)
    {
        _IterationResultGrpcClient = IterationResultGrpcClient;
    }

    public async Task<List<IterationResultDto>> Handle(GetIterationResultsBySimulationIdQuery request, CancellationToken cancellationToken)
    {
        var iterationResults = await _IterationResultGrpcClient.GetIterationResultsBySimulationIdAsync(request.simulationId, cancellationToken);
        return iterationResults;
    }
}
