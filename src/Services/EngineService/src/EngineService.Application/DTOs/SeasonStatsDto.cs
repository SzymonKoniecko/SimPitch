using System;

namespace EngineService.Application.DTOs;

public class SeasonStatsDto
{
    public Guid TeamId { get; set; }
    public string SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public float LeagueStrength { get; set; }
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
}
