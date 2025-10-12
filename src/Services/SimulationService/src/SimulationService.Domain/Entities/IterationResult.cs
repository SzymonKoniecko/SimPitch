using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Entities;

public class IterationResult
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int IterationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public string SimulatedMatchRounds { get; set; }
    public float LeagueStrength { get; set; }
    public float PriorLeagueStrength { get; set; }
    public string SimulationParams { get; set; }
    public string Raport { get; set; }
}