using System;

namespace SimulationService.Application.Features.Simulations.DTOs;

public class SimulationParamsDto
{
    public string Title { get; set; }
    public List<string> SeasonYears { get; set; }
    public Guid LeagueId { get; set; }
    public int Iterations { get; set; }
    public Guid LeagueRoundId { get; set; }
    public bool CreateScoreboardOnCompleteIteration { get; set; }
    public int Seed { get; set; }
    public int GamesToReachTrust { get; set; }
    public float ConfidenceLevel { get; set; }
    public float HomeAdvantage { get; set; }
    public float NoiseFactor { get; set; }
}