using System;

namespace EngineService.Application.DTOs;

public class SimulationOverviewDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
    public SimulationStateDto State { get; set; }
}