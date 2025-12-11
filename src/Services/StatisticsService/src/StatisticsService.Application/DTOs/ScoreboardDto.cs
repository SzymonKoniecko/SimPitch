using System;

namespace StatisticsService.Application.DTOs;

public class ScoreboardDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public Guid IterationResultId { get; set; }
    public List<ScoreboardTeamStatsDto> ScoreboardTeams { get; set; } = new();
    public List<ScoreboardTeamStatsDto> InitialScoreboardTeams { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}