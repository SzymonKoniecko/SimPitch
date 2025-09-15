using System;

namespace StatisticsService.Domain.Entities;

public class Scoreboard
{
    public Guid Id { get; private set; }
    public Guid SimulationId { get; private set; }
    public Guid SimulationResultId { get; private set; }
    private readonly List<ScoreboardTeamStats> _teams = new();
    public IReadOnlyCollection<ScoreboardTeamStats> ScoreboardTeams => _teams.AsReadOnly();
    public float LeagueStrength { get; private set; }
    public float PriorLeagueStrength { get; private set; }
    public DateTime CreatedAt { get; set; }

    private Scoreboard() { }

    public Scoreboard(Guid id, Guid simulationId, Guid simulationResultId, float leagueStrength, float priorLeagueStrength, DateTime createdAt)
    {
        Id = id;
        SimulationId = simulationId;
        SimulationResultId = simulationResultId;
        LeagueStrength = leagueStrength;
        PriorLeagueStrength = priorLeagueStrength;
        CreatedAt = createdAt;
    }

    public void AddTeam(ScoreboardTeamStats team) => _teams.Add(team);
    public void AddTeamRange(IEnumerable<ScoreboardTeamStats> teams) => _teams.AddRange(teams);

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

    // sortowanie w miejscu (in-place) — NIE tworzy nowej listy
    public void SortByCriteria()
    {
        _teams.Sort((a, b) =>
        {
            // po punktach malejąco
            int cmp = b.Points.CompareTo(a.Points);
            if (cmp != 0) return cmp;

            // przy remisie po punktach — zwykle więcej wygranych jest lepsze => malejąco
            cmp = b.Wins.CompareTo(a.Wins);
            return cmp;
        });
    }

    public void SortByRank()
    {
        _teams.Sort((a, b) => a.Rank.CompareTo(b.Rank));
    }
}
