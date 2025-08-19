using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Services;

public class SeasonStatsService
{
    /// <summary>
    /// Calculates the season statistics for the current season based on the provided match rounds.
    /// </summary>
    public SeasonStats CalculateSeasonStatsForCurrentSeasonAsync(MatchRound matchRound, SeasonStats seasonStats, bool isHomeTeam)
    {
        seasonStats.MatchesPlayed++;
        if (isHomeTeam)
        {
            if (matchRound.HomeGoals > matchRound.AwayGoals)
                seasonStats.Wins++;
            else if (matchRound.HomeGoals < matchRound.AwayGoals)
                seasonStats.Losses++;
            else
                seasonStats.Draws++;
            seasonStats.GoalsFor += matchRound.HomeGoals;
            seasonStats.GoalsAgainst += matchRound.AwayGoals;
        }
        else
        {
            if (!isHomeTeam && matchRound.HomeGoals > matchRound.AwayGoals)
                seasonStats.Losses++;
            else if (!isHomeTeam && matchRound.HomeGoals < matchRound.AwayGoals)
                seasonStats.Wins++;
            else
                seasonStats.Draws++;
            seasonStats.GoalsAgainst += matchRound.HomeGoals;
            seasonStats.GoalsFor += matchRound.AwayGoals;
        }
        
        return seasonStats;
    }
}
