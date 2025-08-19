using System;

namespace SimulationService.Domain.ValueObjects;

public class TeamsStrength
{
    public TeamsStrength HomeTeam { get; set; } = new();
    public TeamsStrength AwayTeam { get; set; } = new();
}

public class TeamStrength
{
    public Guid TeamId { get; set; }
    public float Offensive { get; set; }
    public float Defensive { get; set; }
    public float ExpectedGoals { get; set; }
    public bool IsHome { get; set; }
}