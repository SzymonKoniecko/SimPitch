using System;
using StatisticsService.Application.Features.LeagueRounds.DTOs;

namespace StatisticsService.Application.Helpers;

public static class LeagueRoundSimulationHelper
{
    public static List<LeagueRoundDto> FilterLeagueRoundsForCustomSimulationToFindLastLeagueRoundToPlay(
        List<LeagueRoundDto> leagueRounds,
        Guid? lastLeagueRoundToPlay)
    {
        if (lastLeagueRoundToPlay == null || lastLeagueRoundToPlay == Guid.Empty)
            return leagueRounds;

        List<LeagueRoundDto> filteredLeagueRounds = new();

        foreach (var leagueRound in leagueRounds.OrderBy(x => x.Round))
        {
            if (leagueRound.Id == lastLeagueRoundToPlay)
            {
                filteredLeagueRounds.Add(leagueRound);
                return filteredLeagueRounds;
            }
            else
                filteredLeagueRounds.Add(leagueRound);
        }
        return filteredLeagueRounds;
    }
}