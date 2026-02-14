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
    public static TeamStrength Create(Guid Id, Guid teamId, SeasonEnum seasonYear, Guid leagueId, float leagueStrength)
    {
        var seasonStats = SeasonStats.CreateNew(Id, teamId, seasonYear, leagueId, leagueStrength);
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
        // Zabezpieczenie
        if (simulationParams.GamesToReachTrust <= 0)
            throw new ArgumentException("GamesToReachTrust must be greater than zero.");

        float beta_0 = simulationParams.GamesToReachTrust; // * simulationParams.ConfidenceLevel

        SeasonStats.LeagueStrength = (leagueStrength + SeasonStats.LeagueStrength) / 2;

        float alpha_0 = beta_0 * SeasonStats.LeagueStrength;

        // Zaktualizuj parametry posterior
        float posterior_alpha_offensive = alpha_0 + SeasonStats.GoalsFor;
        float posterior_alpha_defensive = alpha_0 + SeasonStats.GoalsAgainst;
        float posterior_beta = beta_0 + SeasonStats.MatchesPlayed;

        var posterior = (
            Offensive: posterior_alpha_offensive / posterior_beta,
            Defensive: posterior_alpha_defensive / posterior_beta
        );

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

    public static List<TeamStrength> MergeAll(IEnumerable<TeamStrength> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        using var enumerator = items.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new InvalidOperationException("Cannot merge empty collection.");

        var first = enumerator.Current;

        float likelihoodOff = 0f;
        float likelihoodDef = 0f;
        float posteriorOff = 0f;
        float posteriorDef = 0f;
        float expectedGoals = 0f;

        int count = 0;

        SeasonStats? seasonAccumulator = null;

        do
        {
            var current = enumerator.Current;

            if (current.TeamId != first.TeamId)
                throw new InvalidOperationException(
                    $"Cannot merge TeamStrength for different teams: {first.TeamId} != {current.TeamId}");

            likelihoodOff += current.Likelihood.Offensive;
            likelihoodDef += current.Likelihood.Defensive;
            posteriorOff += current.Posterior.Offensive;
            posteriorDef += current.Posterior.Defensive;
            expectedGoals += current.ExpectedGoals;

            seasonAccumulator = seasonAccumulator == null
                ? current.SeasonStats
                : seasonAccumulator.Merge(seasonAccumulator, current.SeasonStats);

            count++;

        } while (enumerator.MoveNext());

        return new List<TeamStrength>()
            {first with
            {
                Likelihood = new(
                    likelihoodOff / count,
                    likelihoodDef / count
                ),
                Posterior = new(
                    posteriorOff / count,
                    posteriorDef / count
                ),
                ExpectedGoals = expectedGoals / count,
                LastUpdate = DateTime.Now,
                RoundId = null,
                SeasonStats = seasonAccumulator!
        }};
    }


    /// <summary>
    /// Id used as marker to indicate the INITIAL team SeasonStats BEFORE the simualtion
    /// (LeagueRoundId or LeagueId)
    /// If its a continuation of simulation - will be marked as Guid.Empty (when simulation is finished, that Guid.Empty is filtered out in mapper)  
    /// --- SO NEXT TEAM STRENGTH CANNOT HAVE LEAGUEID OR LEAGUEROUNDID
    /// </summary>
    public TeamStrength EnsureThatUpdatedTeamStrengthNotHaveInitialStatsId()
    {
        var newSeasonStats = this.SeasonStats;
        newSeasonStats.Id = Guid.Empty;

        return this with { SeasonStats = newSeasonStats };
    }

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
