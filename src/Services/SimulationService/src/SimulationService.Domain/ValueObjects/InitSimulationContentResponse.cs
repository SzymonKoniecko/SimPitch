using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.ValueObjects;

public class InitSimulationContentResponse
{
    public Dictionary<Guid, SeasonStats> SeasonStatsDictionary { get; set; } = new();
    public List<MatchRound> MatchRoundsToSimulate { get; set; } = new();
}
