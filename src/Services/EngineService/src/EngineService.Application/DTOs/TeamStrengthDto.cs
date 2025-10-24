using System;
using Newtonsoft.Json;

namespace EngineService.Application.DTOs;

public class TeamStrengthDto
{
    public Guid TeamId { get; set; }
    public StrengthItemDto Likelihood { get; set; }
    public StrengthItemDto Posterior { get; set; }
    public float ExpectedGoals { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    /// <summary>
    /// Indicate the roundId in which these stats has been updated
    /// 
    /// If null, its before the first match
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid? RoundId { get; set; } = null;
    public SeasonStatsDto SeasonStats { get; set; }
}
