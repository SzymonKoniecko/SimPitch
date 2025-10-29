using System;

namespace StatisticsService.Domain.ValueObjects;

public class SimulationOverview
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimulationParams SimulationParams { get; set; }
}