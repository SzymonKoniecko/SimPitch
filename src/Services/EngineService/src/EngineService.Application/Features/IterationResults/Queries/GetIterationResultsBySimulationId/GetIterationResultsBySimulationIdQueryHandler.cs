using System;
using MediatR;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using EngineService.Application.Common.Pagination;

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
        var offset = (request.pageNumber - 1) * request.pageSize;

        var response = await _IterationResultGrpcClient.GetIterationResultsBySimulationIdAsync(request.simulationId, offset, request.pageSize, cancellationToken);
        return new PagedResponse<IterationResultDto>
        {
            Items = response.Item1,
            TotalCount = response.Item2,
            PageNumber = request.pageNumber,
            PageSize = request.pageSize
        };
    }
}
