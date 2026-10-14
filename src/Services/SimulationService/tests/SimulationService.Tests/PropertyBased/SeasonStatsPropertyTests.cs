using FsCheck;
using FsCheck.Xunit;
using SimulationService.Domain.Consts;

namespace SimulationService.Tests.PropertyBased;

public class SeasonStatsPropertyTests
{
    [Property]
    public bool Increment_AddsOneMatch_AndExactlyOneResult(int seed, bool isHomeTeam)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f);
        var match = DomainFakers.PlayedMatch(f, stats.TeamId, f.Random.Guid());

        var result = stats.Increment(match, isHomeTeam);

        return result.MatchesPlayed == stats.MatchesPlayed + 1
            && result.Wins + result.Draws + result.Losses == stats.Wins + stats.Draws + stats.Losses + 1;
    }

    [Property]
    public bool Increment_AccumulatesGoals_FromTeamPerspective(int seed, bool isHomeTeam)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f);
        var match = DomainFakers.PlayedMatch(f, stats.TeamId, f.Random.Guid());

        var result = stats.Increment(match, isHomeTeam);

        int scored = isHomeTeam ? match.HomeGoals!.Value : match.AwayGoals!.Value;
        int conceded = isHomeTeam ? match.AwayGoals!.Value : match.HomeGoals!.Value;
        return result.GoalsFor == stats.GoalsFor + scored
            && result.GoalsAgainst == stats.GoalsAgainst + conceded;
    }

    [Property]
    public bool Increment_HomeAndAwayOutcomesAreMirrored(int seed)
    {
        var f = DomainFakers.Create(seed);
        var home = DomainFakers.SeasonStats(f);
        var away = DomainFakers.SeasonStats(f);
        var match = DomainFakers.PlayedMatch(f, home.TeamId, away.TeamId);

        var homeAfter = home.Increment(match, isHomeTeam: true);
        var awayAfter = away.Increment(match, isHomeTeam: false);

        bool winsMirrorLosses = homeAfter.Wins - home.Wins == awayAfter.Losses - away.Losses
            && homeAfter.Losses - home.Losses == awayAfter.Wins - away.Wins
            && homeAfter.Draws - home.Draws == awayAfter.Draws - away.Draws;

        // Goal difference is a zero-sum game between the two teams of a match
        int goalDifferenceDelta = (homeAfter.GetGoalsDifference() - home.GetGoalsDifference())
            + (awayAfter.GetGoalsDifference() - away.GetGoalsDifference());

        return winsMirrorLosses && goalDifferenceDelta == 0;
    }

    [Property]
    public bool Increment_DoesNotMutateOriginal(int seed, bool isHomeTeam)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f);
        var snapshot = stats with { };

        stats.Increment(DomainFakers.PlayedMatch(f, stats.TeamId, f.Random.Guid()), isHomeTeam);

        return stats == snapshot;
    }

    [Property]
    public bool Increment_Throws_WhenGoalsAreMissing(int seed, bool isHomeTeam)
    {
        var f = DomainFakers.Create(seed);
        var stats = DomainFakers.SeasonStats(f);
        var match = DomainFakers.PlayedMatch(f, stats.TeamId, f.Random.Guid());
        match.AwayGoals = null;

        return Record.Exception(() => stats.Increment(match, isHomeTeam)) is ArgumentNullException;
    }

    [Property]
    public bool Merge_SumsAllCounters(int seed)
    {
        var f = DomainFakers.Create(seed);
        var accumulator = DomainFakers.SeasonStats(f);
        var newData = DomainFakers.SeasonStats(f, accumulator.TeamId);

        var merged = accumulator.Merge(accumulator, newData);

        return merged.MatchesPlayed == accumulator.MatchesPlayed + newData.MatchesPlayed
            && merged.Wins == accumulator.Wins + newData.Wins
            && merged.Draws == accumulator.Draws + newData.Draws
            && merged.Losses == accumulator.Losses + newData.Losses
            && merged.GoalsFor == accumulator.GoalsFor + newData.GoalsFor
            && merged.GoalsAgainst == accumulator.GoalsAgainst + newData.GoalsAgainst;
    }

    [Property]
    public bool Merge_KeepsMatchesConsistentWithResults(int seed)
    {
        var f = DomainFakers.Create(seed);
        var accumulator = DomainFakers.SeasonStats(f);
        var newData = DomainFakers.SeasonStats(f, accumulator.TeamId);

        var merged = accumulator.Merge(accumulator, newData);

        return merged.MatchesPlayed == merged.Wins + merged.Draws + merged.Losses;
    }

    [Property]
    public bool Merge_AlwaysTargetsCurrentSeason_AndNewLeague(int seed)
    {
        var f = DomainFakers.Create(seed);
        var accumulator = DomainFakers.SeasonStats(f);
        var newData = DomainFakers.SeasonStats(f, accumulator.TeamId);

        var merged = accumulator.Merge(accumulator, newData);

        return merged.SeasonYear == SimulationConsts.CURRENT_SEASON
            && merged.LeagueId == newData.LeagueId
            && merged.TeamId == accumulator.TeamId;
    }

    [Property]
    public bool Merge_Throws_ForDifferentTeams(int seed)
    {
        var f = DomainFakers.Create(seed);
        var first = DomainFakers.SeasonStats(f);
        var second = DomainFakers.SeasonStats(f);

        return Record.Exception(() => first.Merge(first, second)) is InvalidOperationException;
    }
}
