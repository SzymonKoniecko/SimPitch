using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.Features.LeagueRound.DTOs;

public class LeagueRoundFilterDto
{
    public string SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public Guid LeagueRoundId { get; set; } = Guid.Empty;
}
