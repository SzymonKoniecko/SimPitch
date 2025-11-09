using System;

namespace StatisticsService.Application.DTOs.Clients;

public class SimulationOverviewDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
}