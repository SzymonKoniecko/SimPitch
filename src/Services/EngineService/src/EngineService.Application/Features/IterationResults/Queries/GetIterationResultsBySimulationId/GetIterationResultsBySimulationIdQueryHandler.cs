using System;
using MediatR;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using EngineService.Application.Common.Pagination;
using EngineService.Application.Mappers;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public class GetIterationResultsBySimulationIdQueryHandler : IRequestHandler<GetIterationResultsBySimulationIdQuery, PagedResponse<IterationResultDto>>
{
    private readonly IIterationResultGrpcClient _IterationResultGrpcClient;

    public GetIterationResultsBySimulationIdQueryHandler(IIterationResultGrpcClient IterationResultGrpcClient)
    {
        _IterationResultGrpcClient = IterationResultGrpcClient;
    }

    public async Task<PagedResponse<IterationResultDto>> Handle(GetIterationResultsBySimulationIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _IterationResultGrpcClient.GetIterationResultsBySimulationIdAsync(request.simulationId, request.PagedRequest, cancellationToken);
        return PagedResponseMapper<IterationResultDto>.ToPagedResponse(response.Item1, response.Item2);
    }
}
