using System;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Simulations.DTOs;

public class SimulationOverviewDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
    public List<LeagueStrengthDto> LeagueStrengths { get; set; }
    public float PriorLeagueStrength { get; set; }
}
