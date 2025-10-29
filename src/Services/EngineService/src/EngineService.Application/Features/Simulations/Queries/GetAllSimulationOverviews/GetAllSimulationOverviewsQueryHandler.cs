using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;

public class GetAllSimulationOverviewsQueryHandler : IRequestHandler<GetAllSimulationOverviewsQuery, PagedResponse<SimulationOverviewDto>>
{
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public GetAllSimulationOverviewsQueryHandler(ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }

    public async Task<PagedResponse<SimulationOverviewDto>> Handle(GetAllSimulationOverviewsQuery query, CancellationToken cancellationToken)
    {
        var offset = (query.pageNumber - 1) * query.pageSize;

        var result = await _simulationEngineGrpcClient.GetPagedSimulationOverviewsAsync(offset, query.pageSize, cancellationToken);

        foreach (var overview in result.Item1)
        {
            overview.State = await _simulationEngineGrpcClient.GetSimulationStateAsync(overview.Id, cancellationToken);
        }
        return new PagedResponse<SimulationOverviewDto>
        {
            Items = result.Item1,
            TotalCount = result.Item2,
            PageNumber = query.pageNumber,
            PageSize = query.pageSize
        };
    }
}
