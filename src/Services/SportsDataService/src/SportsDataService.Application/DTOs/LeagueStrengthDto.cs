using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.DTOs;

public class LeagueStrengthDto
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public SeasonEnum SeasonYear { get; set; }
    public float Strength { get; set; }
}
