using System;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Domain.Services;

public class ScoreboardTeamStatsService
{
    public ScoreboardTeamStatsService()
    {

    }

    public ScoreboardTeamStats CalculateScoreboardTeamStatsForSingleTeam(Guid scoreboardId, Guid teamId, List<MatchRound> matchRounds)
    {
        IEnumerable<MatchRound> homeMatches = matchRounds.Where(x => x.HomeTeamId == teamId);
        IEnumerable<MatchRound> awayMatches = matchRounds.Where(x => x.AwayTeamId == teamId);

        int rank = 0;
        int points = 0;
        int matchPlayed = 0;
        int wins = 0;
        int losses = 0;
        int draws = 0;
        int goalsFor = 0;
        int goalsAgainst = 0;

        foreach (var homeMatch in homeMatches)
        {
            matchPlayed++;
            goalsFor += homeMatch.HomeGoals;
            goalsAgainst += homeMatch.AwayGoals;

            if (homeMatch.IsDraw)
            {
                draws++;
                points++;
            }

            if (homeMatch.HomeGoals > homeMatch.AwayGoals)
            {
                wins++;
                points += 3;
            }

            if (homeMatch.HomeGoals < homeMatch.AwayGoals)
                losses++;
        }

        foreach (var awayMatch in awayMatches)
        {
            matchPlayed++;
            goalsFor += awayMatch.AwayGoals;
            goalsAgainst += awayMatch.HomeGoals;

            if (awayMatch.IsDraw)
            {
                draws++;
                points++;
            }

            if (awayMatch.HomeGoals > awayMatch.AwayGoals)
            {
                losses++;
            }

            if (awayMatch.HomeGoals < awayMatch.AwayGoals)
            {
                wins++;
                points += 3;
            }
        }

        return new ScoreboardTeamStats(Guid.NewGuid(), scoreboardId, teamId, rank, points, matchPlayed, wins, losses, draws, goalsFor, goalsAgainst);
    }
}
