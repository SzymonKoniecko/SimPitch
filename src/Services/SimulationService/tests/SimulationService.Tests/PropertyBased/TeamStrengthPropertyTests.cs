using FsCheck;
using FsCheck.Xunit;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.PropertyBased;

public class TeamStrengthPropertyTests
{
    private const float Tolerance = 1e-4f;

    private static TeamStrength WithStats(SeasonStats stats) =>
        TeamStrength.Create(stats.Id, stats.TeamId, stats.SeasonYear, stats.LeagueId, stats.LeagueStrength)
            .WithSeasonStats(stats);

    [Property]
    public bool WithLikelihood_IsAverageGoalsPerMatch(int seed)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f, minMatches: 1);

        var likelihood = WithStats(stats).WithLikelihood().Likelihood;

        return Math.Abs(likelihood.Offensive - stats.GoalsFor / (float)stats.MatchesPlayed) < Tolerance
            && Math.Abs(likelihood.Defensive - stats.GoalsAgainst / (float)stats.MatchesPlayed) < Tolerance;
    }

    [Property]
    public bool WithLikelihood_Throws_WithoutMatches(int seed)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f) with { MatchesPlayed = 0 };

        return Record.Exception(() => WithStats(stats).WithLikelihood()) is InvalidOperationException;
    }

    /// <summary>
    /// Bayesian shrinkage: the posterior is a weighted average of the league prior and the team's own
    /// goals per match, so it always lies between them.
    /// </summary>
    [Property]
    public bool WithPosterior_LiesBetweenLeaguePriorAndTeamAverage(int seed)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f, minMatches: 1);
        var simulationParams = DomainFakers.SimulationParams(f);
        float leagueStrength = f.Random.Float(1.5f, 3.5f);
        // WithPosterior blends the given league strength into the stats before using it as the prior
        float prior = (leagueStrength + stats.LeagueStrength) / 2;

        var posterior = WithStats(stats).WithLikelihood().WithPosterior(leagueStrength, simulationParams).Posterior;

        float offensiveAverage = stats.GoalsFor / (float)stats.MatchesPlayed;
        float defensiveAverage = stats.GoalsAgainst / (float)stats.MatchesPlayed;
        return IsBetween(posterior.Offensive, prior, offensiveAverage)
            && IsBetween(posterior.Defensive, prior, defensiveAverage);
    }

    [Property]
    public bool WithPosterior_MoreTrustGamesPullsTowardsPrior(int seed, PositiveInt extraTrustGames)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f, minMatches: 1);
        var lowTrust = DomainFakers.SimulationParams(f);
        var highTrust = DomainFakers.SimulationParams(f);
        highTrust.GamesToReachTrust = lowTrust.GamesToReachTrust + extraTrustGames.Get;
        float leagueStrength = f.Random.Float(1.5f, 3.5f);
        float prior = (leagueStrength + stats.LeagueStrength) / 2;

        // WithPosterior mutates the stats it works on, so every calculation gets its own copy
        var low = WithStats(stats with { }).WithLikelihood().WithPosterior(leagueStrength, lowTrust).Posterior;
        var high = WithStats(stats with { }).WithLikelihood().WithPosterior(leagueStrength, highTrust).Posterior;

        return Math.Abs(high.Offensive - prior) <= Math.Abs(low.Offensive - prior) + Tolerance
            && Math.Abs(high.Defensive - prior) <= Math.Abs(low.Defensive - prior) + Tolerance;
    }

    [Property]
    public bool WithPosterior_SetsExpectedGoalsToPosteriorOffensive(int seed)
    {
        var f = DomainFakers.Create(seed);
        var team = DomainFakers.TeamStrength(f, f.Random.Guid(), f.Random.Float(1.5f, 3.5f), DomainFakers.SimulationParams(f));

        return team.Posterior.Offensive > 0 && team.ExpectedGoals == team.Posterior.Offensive;
    }

    [Property]
    public bool WithPosterior_Throws_WhenGamesToReachTrustIsNotPositive(int seed, NonNegativeInt gamesToReachTrust)
    {
        var f = DomainFakers.Create(seed);
        var simulationParams = DomainFakers.SimulationParams(f);
        simulationParams.GamesToReachTrust = -gamesToReachTrust.Get;

        var team = WithStats(DomainFakers.SeasonStats(f, minMatches: 1)).WithLikelihood();

        return Record.Exception(() => team.WithPosterior(2.5f, simulationParams)) is ArgumentException;
    }

    [Property]
    public bool MergeAll_OfCopies_KeepsAverageStrength_AndSumsStats(int seed, PositiveInt copies)
    {
        var f = DomainFakers.Create(seed);
        var team = DomainFakers.TeamStrength(f, f.Random.Guid(), f.Random.Float(1.5f, 3.5f), DomainFakers.SimulationParams(f));
        int count = copies.Get % 5 + 1;

        var merged = TeamStrength.MergeAll(Enumerable.Range(0, count).Select(_ => team.DeepClone())).Single();

        return Math.Abs(merged.Posterior.Offensive - team.Posterior.Offensive) < Tolerance
            && Math.Abs(merged.Posterior.Defensive - team.Posterior.Defensive) < Tolerance
            && Math.Abs(merged.Likelihood.Offensive - team.Likelihood.Offensive) < Tolerance
            && Math.Abs(merged.ExpectedGoals - team.ExpectedGoals) < Tolerance
            && merged.SeasonStats.MatchesPlayed == team.SeasonStats.MatchesPlayed * count
            && merged.RoundId == null;
    }

    [Property]
    public bool MergeAll_Throws_ForDifferentTeams(int seed)
    {
        var f = DomainFakers.Create(seed);
        var simulationParams = DomainFakers.SimulationParams(f);
        var first = DomainFakers.TeamStrength(f, f.Random.Guid(), 2.5f, simulationParams);
        var second = DomainFakers.TeamStrength(f, f.Random.Guid(), 2.5f, simulationParams);

        return Record.Exception(() => TeamStrength.MergeAll(new[] { first, second })) is InvalidOperationException;
    }

    [Fact]
    public void MergeAll_Throws_ForEmptyCollection()
    {
        Assert.Throws<InvalidOperationException>(() => TeamStrength.MergeAll(Array.Empty<TeamStrength>()));
    }

    private static bool IsBetween(float value, float a, float b) =>
        value >= Math.Min(a, b) - Tolerance && value <= Math.Max(a, b) + Tolerance;
}
