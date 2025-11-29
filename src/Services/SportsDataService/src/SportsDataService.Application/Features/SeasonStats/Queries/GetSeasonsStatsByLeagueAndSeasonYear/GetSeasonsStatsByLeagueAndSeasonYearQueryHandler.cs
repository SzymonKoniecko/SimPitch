using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonsStatsByLeagueAndSeasonYear;

public class GetSeasonsStatsByLeagueAndSeasonYearQueryHandler : IRequestHandler<GetSeasonsStatsByLeagueAndSeasonYearQuery, List<SeasonStatsDto>>
{
    private readonly ITeamReadRepository _teamReadRepository;
    private readonly ISeasonStatsReadRepository _seasonStatsReadRepository;

    public GetSeasonsStatsByLeagueAndSeasonYearQueryHandler(ITeamReadRepository teamReadRepository, ISeasonStatsReadRepository seasonStatsReadRepository)
    {
        _teamReadRepository = teamReadRepository;
        _seasonStatsReadRepository = seasonStatsReadRepository;
    }

    public async Task<List<SeasonStatsDto>> Handle(GetSeasonsStatsByLeagueAndSeasonYearQuery query, CancellationToken cancellationToken)
    {
        List<SeasonStatsDto> neededSeasonStats = new();

        var teams = await _teamReadRepository.GetAllTeamsAsync(cancellationToken);
        teams = teams.Where(x => x.LeagueId == query.leagueId);
        
        foreach (var team in teams)
        {
            var stats = await _seasonStatsReadRepository.GetSeasonsStatsByTeamIdAsync(team.Id, cancellationToken);
            neededSeasonStats.AddRange(stats.Where(x => x.SeasonYear == query.seasonYear).Select(x => SeasonStatsMapper.ToDto(x)));
        }

        return neededSeasonStats;
    }
}
