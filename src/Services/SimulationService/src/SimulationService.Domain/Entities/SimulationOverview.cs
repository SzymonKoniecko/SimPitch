using System;

namespace SimulationService.Domain.Entities;

public class SimulationOverview
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string SimulationParams { get; set; }
    public string LeagueStrengthsJSON { get; set; }
    public float PriorLeagueStrength { get; set; }
}