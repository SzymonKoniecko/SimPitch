using System;

namespace SimulationService.Domain.ValueObjects;

public class SimulationParams
{
    public List<string> SeasonYears { get; set; }
    public Guid LeagueId { get; set; }
    public int Iterations { get; set; }
    public Guid LeagueRoundId { get; set; }
}
