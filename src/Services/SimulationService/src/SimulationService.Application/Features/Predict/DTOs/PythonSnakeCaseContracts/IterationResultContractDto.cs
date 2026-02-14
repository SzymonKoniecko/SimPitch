using System;
using Newtonsoft.Json;
using SimulationService.Domain.Enums;

namespace SimulationService.Application.Features.Predict.DTOs.PythonSnakeCaseContracts;
/// <summary>
/// Only to convert from snake_case from python microservice to PascalCase
/// </summary>
public class IterationResultContractDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int IterationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<TeamStrengthContractDto> TeamStrengths { get; set; }
    public List<MatchRoundContractDto> SimulatedMatchRounds { get; set; }

}
public class TeamStrengthContractDto
{
    [JsonProperty("team_id")]
    public Guid TeamId { get; set; }

    [JsonProperty("likelihood")]
    public StrengthItemContractDto Likelihood { get; set; }

    [JsonProperty("posterior")]
    public StrengthItemContractDto Posterior { get; set; }

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
    public SeasonStatsContractDto SeasonStats { get; set; }

}

public class MatchRoundContractDto
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

public class SeasonStatsContractDto
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
public class StrengthItemContractDto
{
    [JsonProperty("offensive")]
    public float Offensive { get; set; }

    [JsonProperty("defensive")]
    public float Defensive { get; set; }
}

