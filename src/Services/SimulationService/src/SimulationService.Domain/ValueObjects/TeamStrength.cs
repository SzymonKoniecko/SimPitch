using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.ValueObjects;

public class TeamStrength
{
    public Guid TeamId { get; set; }
    public float LikelihoodOffensiveStrength { get; set; }
    public float PosteriorOffensiveStrength { get; set; }
    public float LikelihoodDefensiveStrength { get; set; }
    public float PosteriorDefensiveStrength { get; set; }
    /// <summary>
    /// xG - Expected Goals (not developed yet)
    /// </summary>
    public float ExpectedGoals { get; set; }
    public bool IsHome { get; set; }
    public SeasonStats SeasonStats { get; set; }
}