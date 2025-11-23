using System;
using SimulationService.Domain.Consts;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.ValueObjects;

public record TeamStrength
{
    public Guid TeamId { get; init; }
    public (float Offensive, float Defensive) Likelihood { get; init; }
    public (float Offensive, float Defensive) Posterior { get; init; }
    
    /// <summary>
    /// Represents the team's generic expected goals against an average league opponent.
    /// Effectively equal to Posterior.Offensive in this model context.
    /// </summary>
    public float ExpectedGoals { get; init; }
    
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Indicate the roundId in which these stats has been updated
    /// If null, its before the first match
    /// </summary>
    public Guid? RoundId { get; set; } = null;
    
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

    public TeamStrength WithPosterior(float leagueStrength, SimulationParams simulationParams)
    {
        if (SeasonStats.MatchesPlayed == 0)
            throw new InvalidOperationException("Cannot calculate posterior without matches played.");

        // Step 1: Calculate beta_0 based on trust factor
        float beta_0 = (1 - simulationParams.GamesToReachTrust)
                        / simulationParams.GamesToReachTrust
                        * simulationParams.ConfidenceLevel;

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
        // Chain update: Automatically update ExpectedGoals when Posterior is recalculated
        var updatedPosterior = this with { Posterior = posterior };
        return updatedPosterior.WithExpectedGoalsFromPosterior();
    }

    /// <summary>
    /// Sets specific Expected Goals value manually.
    /// </summary>
    public TeamStrength WithExpectedGoals(float expectedGoals)
        => this with { ExpectedGoals = expectedGoals };

    /// <summary>
    /// Sets Expected Goals based on current Posterior Offensive strength.
    /// In Poisson model, Posterior.Offensive represents lambda (avg goals) against average defense.
    /// </summary>
    public TeamStrength WithExpectedGoalsFromPosterior()
    {
        // If Posterior hasn't been calculated (is 0), return current state or calculate default
        if (Posterior.Offensive == 0) 
            return this;

        return this with { ExpectedGoals = Posterior.Offensive };
    }

    public TeamStrength WithSeasonStats(SeasonStats newSeasonStats)
    {
        if (newSeasonStats == null)
            throw new ArgumentNullException(nameof(newSeasonStats));

        return this with { SeasonStats = newSeasonStats };
    }

    public TeamStrength UpdateTime()
        => this with { LastUpdate = DateTime.Now };

    public TeamStrength SetRoundId(Guid roundId)
        => this with { RoundId = roundId };

    public TeamStrength DeepClone()
    {
        return this with
        {
            // `with` copies value types and strings automatically
            // SeasonStats is a reference/record that needs explicit deep clone if it's mutable or nested
            SeasonStats = this.SeasonStats.CloneDeep() 
        };
    }
}
