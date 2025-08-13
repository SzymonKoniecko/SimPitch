using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.Features.LeagueRound.DTOs;

public class LeagueRoundFilterDto
{
    public string SeasonYear { get; set; }
    public int Round { get; set; } = 0;
    public Guid LeagueRoundId { get; set; } = Guid.Empty;
}
