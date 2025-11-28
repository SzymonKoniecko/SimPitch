using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.ValueObjects;

public class SimulationContent
{
    public SimulationParams SimulationParams { get; set; }
    /// <summary>
    /// Key: TeamId;
    /// Value: TeamStrength;
    /// </summary>
    public Dictionary<Guid, List<TeamStrength>> TeamsStrengthDictionary { get; set; } = new();
    public List<MatchRound> MatchRoundsToSimulate { get; set; } = new();
    /// <summary>
    /// Averange team calc by all_goals / all_matches
    /// </summary>
    public float PriorLeagueStrength { get; set; } = 0;
    public List<LeagueStrength> LeagueStrengths { get; set; } = new();
}