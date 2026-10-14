using FsCheck;
using FsCheck.Xunit;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.PropertyBased;

public class MatchSimulatorWorkflowPropertyTests
{
    private static readonly SimulationModelType[] LocalModels =
    {
        SimulationModelType.StandardPoisson,
        SimulationModelType.DixonColes,
        SimulationModelType.BivariatePoisson,
        SimulationModelType.Advanced
    };

    private static (SimulationContent Before, SimulationContent After) RunWorkflow(int seed, NonNegativeInt model, int rngSeed)
    {
        // Bogus with the same seed produces identical leagues, so "before" can be compared with "after"
        var before = DomainFakers.SimulationContent(DomainFakers.Create(seed));
        var content = DomainFakers.SimulationContent(DomainFakers.Create(seed));
        var simulator = new MatchSimulatorService(rngSeed, LocalModels[model.Get % LocalModels.Length]);

        return (before, simulator.SimulationWorkflow(content));
    }

    /// <summary>The newest TeamStrength is the last one appended by the workflow.</summary>
    private static TeamStrength Latest(SimulationContent content, Guid teamId) =>
        content.TeamsStrengthDictionary[teamId].Last();

    [Property(MaxTest = 50)]
    public bool Workflow_PlaysEveryMatch_WithConsistentResult(int seed, NonNegativeInt model, int rngSeed)
    {
        var (_, after) = RunWorkflow(seed, model, rngSeed);

        return after.MatchRoundsToSimulate.All(m =>
            m.IsPlayed
            && m.HomeGoals is >= 0
            && m.AwayGoals is >= 0
            && m.IsDraw == (m.HomeGoals == m.AwayGoals));
    }

    [Property(MaxTest = 50)]
    public bool Workflow_CountsEveryMatchOnceForEachTeam(int seed, NonNegativeInt model, int rngSeed)
    {
        var (before, after) = RunWorkflow(seed, model, rngSeed);

        return before.TeamsStrengthDictionary.Keys.All(teamId =>
        {
            int fixtures = after.MatchRoundsToSimulate.Count(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId);
            var initial = before.TeamsStrengthDictionary[teamId].Single().SeasonStats;
            var final = Latest(after, teamId).SeasonStats;

            return final.MatchesPlayed == initial.MatchesPlayed + fixtures
                && final.Wins + final.Draws + final.Losses == initial.Wins + initial.Draws + initial.Losses + fixtures;
        });
    }

    [Property(MaxTest = 50)]
    public bool Workflow_GoalsScoredByTeams_EqualGoalsInMatches(int seed, NonNegativeInt model, int rngSeed)
    {
        var (before, after) = RunWorkflow(seed, model, rngSeed);

        int goalsInMatches = after.MatchRoundsToSimulate.Sum(m => m.HomeGoals!.Value + m.AwayGoals!.Value);
        int goalsForDelta = before.TeamsStrengthDictionary.Keys.Sum(teamId =>
            Latest(after, teamId).SeasonStats.GoalsFor - before.TeamsStrengthDictionary[teamId].Single().SeasonStats.GoalsFor);
        int goalsAgainstDelta = before.TeamsStrengthDictionary.Keys.Sum(teamId =>
            Latest(after, teamId).SeasonStats.GoalsAgainst - before.TeamsStrengthDictionary[teamId].Single().SeasonStats.GoalsAgainst);

        return goalsForDelta == goalsInMatches && goalsAgainstDelta == goalsInMatches;
    }

    [Property(MaxTest = 50)]
    public bool Workflow_IsDeterministic_ForTheSameSeed(int seed, NonNegativeInt model, int rngSeed)
    {
        var (_, first) = RunWorkflow(seed, model, rngSeed);
        var (_, second) = RunWorkflow(seed, model, rngSeed);

        return first.MatchRoundsToSimulate.Select(m => (m.HomeGoals, m.AwayGoals))
            .SequenceEqual(second.MatchRoundsToSimulate.Select(m => (m.HomeGoals, m.AwayGoals)));
    }
}
