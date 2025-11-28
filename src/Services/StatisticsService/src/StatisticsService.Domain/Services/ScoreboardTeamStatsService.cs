using System;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Domain.Services;

public class ScoreboardTeamStatsService
{
    public ScoreboardTeamStatsService()
    {

    }

    public List<ScoreboardTeamStats> CalculateScoreboardTeamStats(Guid scoreboardId, List<MatchRound> matchRounds)
    {
        Dictionary<Guid, ScoreboardTeamStats> teamStatsDict = new Dictionary<Guid, ScoreboardTeamStats>();
        matchRounds.OrderBy(x => x.RoundId);
        foreach (var match in matchRounds)
        {
            if (match.HomeTeamId == Guid.Parse("a9e2d5f7-1b3c-4e0a-8f6d-7c0b9a1e3d5f") || match.AwayTeamId == Guid.Parse("a9e2d5f7-1b3c-4e0a-8f6d-7c0b9a1e3d5f"))
            {
                System.Console.WriteLine("a9e2d5f7-1b3c-4e0a-8f6d-7c0b9a1e3d5f");
            }
            (ScoreboardTeamStats, ScoreboardTeamStats) stats = CalculateScoreboardTeamStatsForMatch(scoreboardId, match);
            if (teamStatsDict.ContainsKey(stats.Item1.TeamId))
            {
                teamStatsDict[stats.Item1.TeamId].MergeMatchStats(stats.Item1);
            }
            else
            {
                teamStatsDict.Add(stats.Item1.TeamId, stats.Item1);
            }


            if (teamStatsDict.ContainsKey(stats.Item2.TeamId))
            {
                teamStatsDict[stats.Item2.TeamId].MergeMatchStats(stats.Item2);
            }
            else
            {
                teamStatsDict.Add(stats.Item2.TeamId, stats.Item2);
            }
        }

        List<ScoreboardTeamStats> result = new List<ScoreboardTeamStats>();

        foreach (var (key, value) in teamStatsDict)
        {
            result.Add(value);
        }

        return result;
    }

    public (ScoreboardTeamStats, ScoreboardTeamStats) CalculateScoreboardTeamStatsForMatch(Guid scoreboardId, MatchRound match)
    {
        if (match.HomeGoals == null || match.AwayGoals == null)
        {
            throw new ArgumentNullException($"Home goals or away goals are null !! MatchRoundId:{match.Id} " + nameof(CalculateScoreboardTeamStatsForMatch));
        }
        if (match.IsDraw == null)
        {
            throw new ArgumentNullException($"IsDraw flag is null !! MatchRoundId:{match.Id} " + nameof(CalculateScoreboardTeamStatsForMatch));
        }

        int rank = 0; // Rank will be set later
        (int, int) points = (0, 0);
        (int, int) win = (0, 0);
        (int, int) loss = (0, 0);
        (int, int) draws = (0, 0);
        (int, int) goalsFor = (0, 0);
        (int, int) goalsAgainst = (0, 0);

        goalsFor = (match.HomeGoals.Value, match.AwayGoals.Value);
        goalsAgainst = (match.AwayGoals.Value, match.HomeGoals.Value);

        if (match.IsDraw.Value)
        {
            draws = (1, 1);
            points = (1, 1);
        }
        else if (match.HomeGoals > match.AwayGoals)
        {
            win = (1, 0);
            loss = (0, 1);
            points = (3, 0);
        }
        else if (match.HomeGoals < match.AwayGoals)
        {
            win = (0, 1);
            loss = (1, 0);
            points = (0, 3);
        }

        return (new ScoreboardTeamStats(Guid.NewGuid(), scoreboardId, match.HomeTeamId, rank, points.Item1, 1, win.Item1, loss.Item1, draws.Item1, goalsFor.Item1, goalsAgainst.Item1),
            new ScoreboardTeamStats(Guid.NewGuid(), scoreboardId, match.AwayTeamId, rank, points.Item2, 1, win.Item2, loss.Item2, draws.Item2, goalsFor.Item2, goalsAgainst.Item2));
    }
    
}
