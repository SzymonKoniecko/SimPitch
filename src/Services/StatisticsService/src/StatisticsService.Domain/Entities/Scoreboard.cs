using System;

namespace StatisticsService.Domain.Entities;

public class Scoreboard
{
    public Guid Id { get; private set; }
    public Guid SimulationId { get; private set; }
    public Guid IterationResultId { get; private set; }
    private readonly List<ScoreboardTeamStats> _teams = new();
    public IReadOnlyCollection<ScoreboardTeamStats> ScoreboardTeams => _teams.AsReadOnly();
    private readonly List<ScoreboardTeamStats> _teamsInitialStats = new();
    public IReadOnlyCollection<ScoreboardTeamStats> ScoreboardTeamsInitialStats => _teamsInitialStats.AsReadOnly();
    public DateTime CreatedAt { get; set; }

    private Scoreboard() { }

    public Scoreboard(Guid id, Guid simulationId, Guid iterationResultId, DateTime createdAt)
    {
        Id = id;
        SimulationId = simulationId;
        IterationResultId = iterationResultId;
        CreatedAt = createdAt;
    }

    public void AddTeam(ScoreboardTeamStats team) => _teams.Add(team);
    public void AddTeamRange(IEnumerable<ScoreboardTeamStats> teams) => _teams.AddRange(teams);
    public void AddTeamRangeInitialStats(IEnumerable<ScoreboardTeamStats> teams) => _teamsInitialStats.AddRange(teams);

    public void SetInitialStatFlag()
    {
        foreach (var initialStat in _teamsInitialStats)
        {
            initialStat.SetInitialStatAsTrue();
        }
    }

    public void SetRankings()
    {
        // najpierw posortuj poprawnie
        SortByCriteria();

        int rank = 1;
        foreach (var team in _teams)
        {
            team.SetRanking(rank);
            rank++;
        }
    }

    public void SortByCriteria()
    {
        _teams.Sort((a, b) =>
        {
            // 1. Po punktach (malejąco)
            int cmp = b.Points.CompareTo(a.Points);
            if (cmp != 0) return cmp;

            // 2. Po wygranych (malejąco)
            cmp = b.Wins.CompareTo(a.Wins);
            if (cmp != 0) return cmp;

            // 3. Po różnicy bramkowej (malejąco) ← DODANO
            int gdA = a.GoalsFor - a.GoalsAgainst;
            int gdB = b.GoalsFor - b.GoalsAgainst;
            cmp = gdB.CompareTo(gdA);
            if (cmp != 0) return cmp;

            // 4. Po bramkach strzelonych (opcjonalnie)
            return b.GoalsFor.CompareTo(a.GoalsFor);
        });
    }

    public void SortByRank()
    {
        _teams.Sort((a, b) => a.Rank.CompareTo(b.Rank));
    }
}
