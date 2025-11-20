using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Entities;

public class LeagueStrength
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public SeasonEnum SeasonYear { get; set; }
    public float Strength { get; set; }
}
