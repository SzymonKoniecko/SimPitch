using System;

namespace StatisticsService.Domain.Entities;

public class SimulationState
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SimulationId { get; set; }

    public int LastCompletedIteration { get; set; }

    public float ProgressPercent { get; set; }

    public string State { get; set; }

    public DateTime UpdatedAt { get; set; }
}