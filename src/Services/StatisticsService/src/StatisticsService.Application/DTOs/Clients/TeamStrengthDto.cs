using System;

namespace StatisticsService.Application.DTOs.Clients;

public class TeamStrengthDto
{
    public Guid TeamId { get; init; }
    public (float Offensive, float Defensive) Likelihood { get; init; }
    public (float Offensive, float Defensive) Posterior { get; init; }
    public float ExpectedGoals { get; init; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    /// <summary>
    /// Indicate the roundId in which these stats has been updated
    /// 
    /// If null, its before the first match
    /// </summary>
    public Guid? RoundId { get; set; } = null;
    public SeasonStatsDto SeasonStats { get; set; }
}
