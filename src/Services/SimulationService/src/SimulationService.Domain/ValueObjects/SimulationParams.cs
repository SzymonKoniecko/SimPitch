using System;

namespace SimulationService.Domain.ValueObjects;

public class SimulationParams
{
    public string Title { get; set; }
    public List<string> SeasonYears { get; set; }
    public Guid LeagueId { get; set; }
    public int Iterations { get; set; }
    public Guid LeagueRoundId { get; set; }
    public bool CreateScoreboardOnCompleteIteration { get; set; }
}
