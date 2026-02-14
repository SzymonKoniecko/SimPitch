using System;
using Newtonsoft.Json;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class TeamStrengthDto
{
    [JsonProperty("team_id")]
    public Guid TeamId { get; set; }

    [JsonProperty("likelihood")]
    public StrengthItemDto Likelihood { get; set; }

    [JsonProperty("posterior")]
    public StrengthItemDto Posterior { get; set; }

    // jeśli Python wysyła string albo "N/A", to zmień typ na string albo zrób custom converter
    [JsonProperty("expected_goals")]
    public float ExpectedGoals { get; set; }

    [JsonProperty("last_update")]
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    /// <summary>
    /// Indicate the roundId in which these stats has been updated.
    /// If null, it's before the first match.
    /// </summary>
    [JsonProperty("round_id", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? RoundId { get; set; } = null;

    [JsonProperty("season_stats")]
    public SeasonStatsDto SeasonStats { get; set; }
}