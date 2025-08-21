using System;

namespace SimulationService.Domain.Entities;

public class LeagueRound
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
    public int Round { get; set; }
    public int MaxRound { get; set; }
}
