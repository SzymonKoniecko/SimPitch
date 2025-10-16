using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Features.Simulations.Queries;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;

public class GetAllSimulationOverviewsQueryHandler : IRequestHandler<GetAllSimulationOverviewsQuery, IEnumerable<SimulationOverviewDto>>
{
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetAllSimulationOverviewsQueryHandler(ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }

    public async Task<IEnumerable<SimulationOverviewDto>> Handle(GetAllSimulationOverviewsQuery query, CancellationToken cancellationToken)
    {
        var results = await _simulationOverviewReadRepository.GetSimulationOverviewsAsync(cancellationToken);
        return results.Select(x => SimulationOverviewMapper.ToDto(x));
    }
}
