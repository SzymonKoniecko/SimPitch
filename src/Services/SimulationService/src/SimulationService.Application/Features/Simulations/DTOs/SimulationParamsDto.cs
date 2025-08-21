using System;

namespace SimulationService.Application.Features.Simulations.DTOs;

public class SimulationParamsDto
{
    public string SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public Guid RoundId { get; set; }
}