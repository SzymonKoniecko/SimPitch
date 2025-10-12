using System;

namespace StatisticsService.Application.DTOs;

public class ScoreboardDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public Guid IterationResultId { get; set; }
    public List<ScoreboardTeamStatsDto> ScoreboardTeams { get; set; } = new();
    public float LeagueStrength { get; set; }
    public float PriorLeagueStrength { get; set; }
    public DateTime CreatedAt { get; set; }
}