using System;
using Newtonsoft.Json;

namespace SimulationService.Application.Features.MatchRounds.DTOs;

public class MatchRoundDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("round_id")]
    public Guid RoundId { get; set; }

    [JsonProperty("home_team_id")]
    public Guid HomeTeamId { get; set; }

    [JsonProperty("away_team_id")]
    public Guid AwayTeamId { get; set; }

    [JsonProperty("home_goals")]
    public int? HomeGoals { get; set; }

    [JsonProperty("away_goals")]
    public int? AwayGoals { get; set; }

    [JsonProperty("is_draw")]
    public bool? IsDraw { get; set; }

    [JsonProperty("is_played")]
    public bool IsPlayed { get; set; }
}