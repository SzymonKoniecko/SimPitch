using System;

namespace EngineService.Application.DTOs;

public class ScoreboardTeamStatsDto
{
    public Guid Id { get; set; }
    public Guid ScoreboardId { get; set; }
    public Guid TeamId { get; set; }
    public int Rank { get; set; }
    public int Points { get; set; }
    public int MatchPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
}