using System;

namespace StatisticsService.Domain.Entities;

public class ScoreboardTeamStats
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
    public bool IsInitialStat { get; private set; }

    private ScoreboardTeamStats() { }

    public ScoreboardTeamStats(Guid id, Guid scoreboardId, Guid teamId, int rank, int points,
                          int matchPlayed, int wins, int losses, int draws,
                          int goalsFor, int goalsAgainst, bool isInitialStat = false)
    {
        if (wins + draws + losses != matchPlayed)
            throw new ArgumentException(
                $"SUM W+D+L ({wins}+{draws}+{losses}) != MatchPlayed ({matchPlayed}) - id: {id}");

        int expectedPoints = (wins * 3) + (draws * 1);
        if (points != expectedPoints)
            throw new ArgumentException(
                $"Points ({points}) != Expected ({expectedPoints}) - id: {id}");

        if (matchPlayed < 0 || wins < 0 || losses < 0 || draws < 0)
            throw new ArgumentException($"matchPlayed {matchPlayed} or wins {wins} or losses {losses} or draws {draws} - id: {id}");

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
        IsInitialStat = isInitialStat;
    }

    public void SetRanking(int rank)
    {
        this.Rank = rank;
    }

    public void SetInitialStatAsTrue()
    {
        this.IsInitialStat = true;
    }

    public void MergeMatchStats(ScoreboardTeamStats item)
    {
        Points += item.Points;
        MatchPlayed += item.MatchPlayed;
        Wins += item.Wins;
        Losses += item.Losses;
        Draws += item.Draws;
        GoalsFor += item.GoalsFor;
        GoalsAgainst += item.GoalsAgainst;
    }
}
