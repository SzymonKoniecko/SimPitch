using System;

namespace StatisticsService.Application.DTOs.Clients;

public class TeamStrengthDto
{
    public Guid TeamId { get; init; }
    public StrengthItemDto Likelihood { get; init; } = new();
    public StrengthItemDto Posterior { get; init; } = new();
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
