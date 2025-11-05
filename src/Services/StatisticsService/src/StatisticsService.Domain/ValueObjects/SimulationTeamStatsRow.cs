using System;

namespace StatisticsService.Domain.ValueObjects;

public class SimulationTeamStatsRow
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public Guid TeamId { get; set; }
    public string PositionProbbility { get; set; } = string.Empty;
    public float AverangePoints { get; set; }
    public float AverangeWins { get; set; }
    public float AverangeLosses { get; set; }
    public float AverangeDraws { get; set; }
    public float AverangeGoalsFor { get; set; }
    public float AverangeGoalsAgainst { get; set; }
}
