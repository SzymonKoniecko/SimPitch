using System;
using SimulationService.Domain.Constraints;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public record TeamStrength
{
    public Guid TeamId { get; init; }
    public (float Offensive, float Defensive) Likelihood { get; init; }
    public (float Offensive, float Defensive) Posterior { get; init; }
    public float ExpectedGoals { get; init; }
    public SeasonStats SeasonStats { get; set; }

    private TeamStrength(Guid teamId,
                         SeasonStats seasonStats,
                         (float Offensive, float Defensive)? likelihood = null,
                         (float Offensive, float Defensive)? posterior = null,
                         float expectedGoals = 0f)
    {
        TeamId = teamId;
        SeasonStats = seasonStats ?? throw new ArgumentNullException(nameof(seasonStats));
        Likelihood = likelihood ?? default;
        Posterior = posterior ?? default;
        ExpectedGoals = expectedGoals;
    }

    /// <summary>
    /// Factory method to create TeamStrength with SeasonStats.
    /// </summary>
    public static TeamStrength Create(Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength)
    {
        var seasonStats = SeasonStats.CreateNew(teamId, seasonYear, leagueId, leagueStrength);
        return new TeamStrength(teamId, seasonStats);
    }


    public TeamStrength WithLikelihood()
    {
        if (SeasonStats.MatchesPlayed == 0)
            throw new InvalidOperationException("Cannot calculate likelihood without matches played.");

        var likelihood = (
            Offensive: SeasonStats.GoalsFor / (float)SeasonStats.MatchesPlayed,
            Defensive: SeasonStats.GoalsAgainst / (float)SeasonStats.MatchesPlayed
        );

        return this with { Likelihood = likelihood };
    }

    public TeamStrength WithPosterior(float leagueStrength)
    {
        if (SeasonStats.MatchesPlayed == 0)
            throw new InvalidOperationException("Cannot calculate posterior without matches played.");

        // Step 1: Calculate beta_0 based on trust factor
        float beta_0 = (1 - SimulationConstraints.GAMES_TO_REACH_TRUST)
                        / SimulationConstraints.GAMES_TO_REACH_TRUST
                        * SimulationConstraints.SIMULATION_CONFIDENCE_LEVEL;

        // Step 2: Calculate alpha_0 so prior mean matches league mean
        float alpha_0 = beta_0 * leagueStrength;

        // Step 3: Update with observed data
        float posterior_alpha_offensive = alpha_0 + SeasonStats.GoalsFor;
        float posterior_alpha_defensive = alpha_0 + SeasonStats.GoalsAgainst;
        float posterior_beta = beta_0 + SeasonStats.MatchesPlayed;

        var posterior = (
            Offensive: posterior_alpha_offensive / posterior_beta,
            Defensive: posterior_alpha_defensive / posterior_beta
        );

        return this with { Posterior = posterior };
    }

    public TeamStrength WithExpectedGoals(float expectedGoals)
        => this with { ExpectedGoals = expectedGoals };
}
