using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Features.Simulations.Queries;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;

public class GetAllSimulationOverviewsQueryHandler : IRequestHandler<GetAllSimulationOverviewsQuery, (IEnumerable<SimulationOverviewDto>, PagedResponseDetails)>
{
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetAllSimulationOverviewsQueryHandler(ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }

    public async Task<(IEnumerable<SimulationOverviewDto>, PagedResponseDetails)> Handle(GetAllSimulationOverviewsQuery query, CancellationToken cancellationToken)
    {
        var results = await _simulationOverviewReadRepository.GetSimulationOverviewsAsync(
            new PagedRequest(
                query.PagedRequest.Offset,
                query.PagedRequest.PageSize,
                EnumMapper.SortingOptionToEnum(query.PagedRequest.SortingMethod.SortingOption),
                query.PagedRequest.SortingMethod.Order
            ), cancellationToken);

        return
        (
            results.Select(x => SimulationOverviewMapper.ToDto(x)),
            new PagedResponseDetails()
            {
                TotalCount = await _simulationOverviewReadRepository.GetSimulationOverviewCountAsync(cancellationToken),
                PageNumber = (query.PagedRequest.Offset / query.PagedRequest.PageSize) + 1,
                PageSize = query.PagedRequest.PageSize,
                SortingOption = query.PagedRequest.SortingMethod.SortingOption.ToString(),
                Order = query.PagedRequest.SortingMethod.Order
            }
        );
    }
}
