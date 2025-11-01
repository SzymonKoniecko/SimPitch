using System;

namespace SimulationService.Domain.Entities;

public class SimulationOverview
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public string SimulationParams { get; set; }
}