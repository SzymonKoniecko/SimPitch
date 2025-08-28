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

    private Scoreboard() { }

    public Scoreboard(Guid id, Guid simulationId, Guid simulationResultId, float leagueStrength, float priorLeagueStrength)
    {
        Id = id;
        SimulationId = simulationId;
        SimulationResultId = simulationResultId;
        LeagueStrength = leagueStrength;
        PriorLeagueStrength = priorLeagueStrength;
    }

    public void AddTeam(ScoreboardTeamStats team)
    {
        _teams.Add(team);
    }
}