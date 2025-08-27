using System;

namespace StatisticsService.Domain.Entities;

public class ScoreboardTeam
{
    public Guid Id { get; private set; }
    public Guid ScoreboardId { get; private set; }
    public Guid TeamId { get; private set; }
    public int Rank { get; private set; }
    public int Points { get; private set; }
    public int MatchPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Draws { get; private set; }
    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }

    private ScoreboardTeam() { }

    public ScoreboardTeam(Guid id, Guid scoreboardId, Guid teamId, int rank, int points,
                          int matchPlayed, int wins, int losses, int draws,
                          int goalsFor, int goalsAgainst)
    {
        Id = id;
        ScoreboardId = scoreboardId;
        TeamId = teamId;
        Rank = rank;
        Points = points;
        MatchPlayed = matchPlayed;
        Wins = wins;
        Losses = losses;
        Draws = draws;
        GoalsFor = goalsFor;
        GoalsAgainst = goalsAgainst;
    }
}
