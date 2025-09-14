using System;

namespace StatisticsService.Application.Features.LeagueRounds.DTOs;

public class LeagueRoundDtoRequest
{
    public List<string> SeasonYears { get; set; }
    public Guid LeagueId { get; set; }
    public Guid LeagueRoundId { get; set; } = Guid.Empty;
}
