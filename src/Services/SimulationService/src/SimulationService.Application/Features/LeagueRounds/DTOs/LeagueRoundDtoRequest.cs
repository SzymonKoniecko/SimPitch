using System;

namespace SimulationService.Application.Features.LeagueRounds.DTOs;

public class LeagueRoundDtoRequest
{
    public string SeasonYear { get; set; }
    public int Round { get; set; } = 0;
    public Guid LeagueRoundId { get; set; } = Guid.Empty;
    
}
