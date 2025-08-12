using System;

namespace SimulationService.Application.Features.LeagueRounds.DTOs;

public class LeagueRoundDto
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
    public int Round { get; set; }
    public int MaxRound { get; set; }
}
