using System;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Predict.DTOs;

public class PredictRequestDto
{
    public Guid SimulationId { get; set; }
    public Guid LeagueId { get; set; }
    public int IterationCount { get; set; }
    public Dictionary<Guid, List<TeamStrength>> TeamStrengths { get; set; } = new();
    public List<MatchRound> MatchesToSimulate { get; set; } = new();
    public int TrainUntilRoundNumber { get; set; }
    public float? LeagueAverangeStrength { get; set; }
    public int? Seed { get; set; }
    public float? TrainRatio { get; set; }
}
