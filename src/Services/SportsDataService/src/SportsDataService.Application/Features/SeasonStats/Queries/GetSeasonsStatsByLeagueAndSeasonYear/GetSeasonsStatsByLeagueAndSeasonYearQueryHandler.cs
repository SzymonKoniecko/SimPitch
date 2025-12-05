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
    private readonly ICompetitionMembershipReadRepository _competitionMembershipReadRepository;

    public GetSeasonsStatsByLeagueAndSeasonYearQueryHandler(
        ITeamReadRepository teamReadRepository, 
        ISeasonStatsReadRepository seasonStatsReadRepository,
        ICompetitionMembershipReadRepository competitionMembershipReadRepository)
    {
        _teamReadRepository = teamReadRepository;
        _seasonStatsReadRepository = seasonStatsReadRepository;
        _competitionMembershipReadRepository = competitionMembershipReadRepository;
    }

    public async Task<List<SeasonStatsDto>> Handle(GetSeasonsStatsByLeagueAndSeasonYearQuery query, CancellationToken cancellationToken)
    {
        List<SeasonStatsDto> neededSeasonStats = new();

        var teams = await _teamReadRepository.GetAllTeamsAsync(cancellationToken);
        var membershipList = await _competitionMembershipReadRepository.GetAllByLeagueIdAndSeasonYearAsync(query.leagueId, query.seasonYear, cancellationToken);
        
        teams = teams.Where(x => 
            membershipList.Any(y => y.TeamId == x.Id)
        ).ToList();
        
        foreach (var team in teams)
        {
            var stats = await _seasonStatsReadRepository.GetSeasonsStatsByTeamIdAsync(team.Id, cancellationToken);
            neededSeasonStats.AddRange(stats.Where(x => x.SeasonYear == query.seasonYear).Select(x => SeasonStatsMapper.ToDto(x)));
        }

        return neededSeasonStats;
    }
}
