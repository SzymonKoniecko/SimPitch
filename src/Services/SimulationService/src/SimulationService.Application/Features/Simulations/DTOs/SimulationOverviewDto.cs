using System;

namespace SimulationService.Application.Features.Simulations.DTOs;

public class SimulationOverviewDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
}
