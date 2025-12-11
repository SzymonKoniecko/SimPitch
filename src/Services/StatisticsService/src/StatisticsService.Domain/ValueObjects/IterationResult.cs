using System;

namespace StatisticsService.Domain.ValueObjects;

public class IterationResult
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int SimulationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<TeamStrength> TeamStrengths { get; set; }
    public List<MatchRound> SimulatedMatchRounds { get; set; }
}
