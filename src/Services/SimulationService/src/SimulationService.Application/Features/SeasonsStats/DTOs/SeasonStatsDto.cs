using System;
using Newtonsoft.Json;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Features.SeasonsStats.DTOs;

public class SeasonStatsDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("team_id")]
    public Guid TeamId { get; set; }

    [JsonProperty("season_year")]
    public SeasonEnum SeasonYear { get; set; }

    [JsonProperty("league_id")]
    public Guid LeagueId { get; set; }

    [JsonProperty("league_strength")]
    public float LeagueStrength { get; set; }

    [JsonProperty("matches_played")]
    public int MatchesPlayed { get; set; }

    [JsonProperty("wins")]
    public int Wins { get; set; }

    [JsonProperty("losses")]
    public int Losses { get; set; }

    [JsonProperty("draws")]
    public int Draws { get; set; }

    [JsonProperty("goals_for")]
    public int GoalsFor { get; set; }

    [JsonProperty("goals_against")]
    public int GoalsAgainst { get; set; }
}