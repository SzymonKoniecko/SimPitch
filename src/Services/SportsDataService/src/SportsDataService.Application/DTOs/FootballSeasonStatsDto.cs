using System;

namespace SportsDataService.Application.DTOs;

public class FootballSeasonStatsDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public int SeasonYear { get; set; }
    public Guid LeagueId { get; set; }
    public int MatchesPlayed { get; set; } = 0;
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;
    public int Draws { get; set; } = 0;
    public int GoalsFor { get; set; } = 0;
    public int GoalsAgainst { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
