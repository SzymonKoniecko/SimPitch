using System;
using Newtonsoft.Json;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class TeamStrengthDto
{
    public Guid TeamId { get; set; }
    public StrengthItemDto Likelihood { get; set; }
    public StrengthItemDto Posterior { get; set; }
    public float ExpectedGoals { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    /// <summary>
    /// Indicate the roundId in which these stats has been updated.
    /// If null, it's before the first match.
    /// </summary>
    [JsonProperty("RoundId", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? RoundId { get; set; } = null;
    public SeasonStatsDto SeasonStats { get; set; }
}