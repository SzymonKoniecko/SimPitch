using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Features.Simulations.Queries;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;

public class GetAllSimulationOverviewsQueryHandler : IRequestHandler<GetAllSimulationOverviewsQuery, (IEnumerable<SimulationOverviewDto>, int)>
{
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetAllSimulationOverviewsQueryHandler(ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }

    public async Task<(IEnumerable<SimulationOverviewDto>, int)> Handle(GetAllSimulationOverviewsQuery query, CancellationToken cancellationToken)
    {
        var results = await _simulationOverviewReadRepository.GetSimulationOverviewsAsync(query.offset, query.limit, cancellationToken);
        
        return
            (results.Select(x => SimulationOverviewMapper.ToDto(x)),
            await _simulationOverviewReadRepository.GetSimulationOverviewCountAsync(cancellationToken));
    }
}
