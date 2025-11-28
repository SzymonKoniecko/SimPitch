using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Helpers;

public static class LeagueRoundSimulationHelper
{
    /// <summary>
    /// targetLeagueRoundId needs to delcare the rest of league rounds to be marked as not played begining from that Id
    /// </summary>
    /// <returns>List to match league rounds to set the matches as not played</returns>
    public static List<Guid> FindLeagueRoundsForCustomSimulation(
        List<LeagueRound> leagueRounds,
        Guid? targetLeagueRoundId)
    {
        if (targetLeagueRoundId == null || targetLeagueRoundId == Guid.Empty)
            return new List<Guid>();

        var ordered = leagueRounds
            .OrderBy(x => x.Round)
            .ToList();

        var index = ordered.FindIndex(x => x.Id == targetLeagueRoundId);

        if (index == -1)
            return new List<Guid>();

        return ordered
            .Skip(index)
            .Select(x => x.Id)
            .ToList();
    }

    /// <summary>
    /// If LeagueRoundId is provided, we are marking the match rounds as not played begining from that Id
    /// </summary>
    /// <param name="matchRounds">All MatchRounds for league</param>
    /// <param name="roundsToClear">Param from match rounds will be marked as not played</param>
    /// <returns></returns>
    public static List<MatchRound> SetCustomStartToSimulate(
        List<MatchRound> matchRounds,
        List<Guid> roundsToClear)
    {
        foreach (var matchRound in matchRounds)
        {
            if (roundsToClear.Contains(matchRound.RoundId))
                matchRound.SetAsNotPlayed();
        }

        return matchRounds;
    }
}