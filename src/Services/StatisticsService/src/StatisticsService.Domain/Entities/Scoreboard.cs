using System;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Domain.Entities;

public class Scoreboard
{
    public Guid Id { get; private set; }
    public Guid SimulationId { get; private set; }
    
    private readonly List<ScoreboardTeam> _teams = new();
    public IReadOnlyCollection<ScoreboardTeam> ScoreboardTeams => _teams.AsReadOnly();

    public float LeagueStrength { get; private set; }
    public float PriorLeagueStrength { get; private set; }

    // Konstruktor dla ORM / Dapper
    private Scoreboard() { }

    public Scoreboard(Guid id, Guid simulationId, float leagueStrength, float priorLeagueStrength)
    {
        Id = id;
        SimulationId = simulationId;
        LeagueStrength = leagueStrength;
        PriorLeagueStrength = priorLeagueStrength;
    }

    public void AddTeam(ScoreboardTeam team)
    {
        _teams.Add(team);
    }
}