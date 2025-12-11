using System;

namespace SimulationService.Domain.ValueObjects;

public class SimulationParams
{
    public string Title { get; set; }
    public List<string> SeasonYears { get; set; }
    public Guid LeagueId { get; set; }
    public int Iterations { get; set; }
    public Guid LeagueRoundId { get; set; }
    public Guid? TargetLeagueRoundId { get; set; }
    public bool CreateScoreboardOnCompleteIteration { get; set; }
    public int Seed { get; set; }
    public int GamesToReachTrust { get; set; }
    public float ConfidenceLevel { get; set; }
    public float HomeAdvantage { get; set; }
    public float NoiseFactor { get; set; }
    public string ModelType { get; set; }
}
