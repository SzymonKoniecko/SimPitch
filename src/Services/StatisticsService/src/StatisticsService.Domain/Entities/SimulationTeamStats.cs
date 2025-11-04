using System;

namespace StatisticsService.Domain.Entities;

public class SimulationTeamStats
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public Guid TeamId { get; set; }
    public float[] PositionProbbility { get; set; }
    public float AverangePoints { get; set; }
    public float AverangeWins { get; set; }
    public float AverangeLosses { get; set; }
    public float AverangeDraws { get; set; }
    public float AverangeGoalsFor { get; set; }
    public float AverangeGoalsAgainst { get; set; }
}