using System;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Features.Leagues.DTOs;

public class LeagueStrengthDto
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public SeasonEnum SeasonYear { get; set; }
    public float Strength { get; set; }
}
