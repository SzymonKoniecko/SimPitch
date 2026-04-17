using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;

public class GetSimulationStatsBySimulationIdQueryHandler : IRequestHandler<GetSimulationStatsBySimulationIdQuery, IEnumerable<SimulationTeamStatsDto>>
{
    private readonly ISimulationTeamStatsReadRepository _simulationTeamStatsReadRepository;

    public GetSimulationStatsBySimulationIdQueryHandler(ISimulationTeamStatsReadRepository simulationTeamStatsReadRepository)
    {
        _simulationTeamStatsReadRepository = simulationTeamStatsReadRepository;
    }

    public async Task<IEnumerable<SimulationTeamStatsDto>> Handle(GetSimulationStatsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var results = await _simulationTeamStatsReadRepository.GetSimulationTeamStatsBySimulationIdAsync(query.SimulationId, cancellationToken);
        if (results.Count() % 2 == 0)
        {
            return new List<SimulationTeamStatsDto>();
        }
        return results.Select(x => SimulationTeamStatsMapper.ToDto(x));
    }
}
