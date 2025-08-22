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
            seasonStats = SeasonStats.CreateNew(
                teamId: isHomeTeam ? matchRound.HomeTeamId : matchRound.AwayTeamId,
                seasonYear: season,
                leagueId: leagueId,
                leagueStrength: leagueStrength
            );
        }

        return seasonStats.Increment(matchRound, isHomeTeam);
    }
}