using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services;

public class SeasonStatsService
{
    public SeasonStats CalculateSeasonStats(MatchRound matchRound, SeasonStats seasonStats, SeasonEnum season, Guid leagueId, float leagueStrength, bool isHomeTeam)
    {
        if (seasonStats == null)
        {
            var newLeagueStrength = (leagueStrength * seasonStats.LeagueStrength) / 2;
            seasonStats = SeasonStats.CreateNew(
                seasonStats.Id,
                teamId: isHomeTeam ? matchRound.HomeTeamId : matchRound.AwayTeamId,
                seasonYear: season,
                leagueId: leagueId,
                leagueStrength: newLeagueStrength
            );
        }

        return seasonStats.Increment(matchRound, isHomeTeam);
    }
}