using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using EngineService.Application.Mappers;
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
        var result = await _simulationEngineGrpcClient.GetPagedSimulationOverviewsAsync(query.PagedRequest, cancellationToken);

        foreach (var overview in result.Item1)
        {
            overview.State = await _simulationEngineGrpcClient.GetSimulationStateAsync(overview.Id, cancellationToken);
        }
        return PagedResponseMapper<SimulationOverviewDto>.ToPagedResponse(result.Item1, result.Item2);
    }
}
