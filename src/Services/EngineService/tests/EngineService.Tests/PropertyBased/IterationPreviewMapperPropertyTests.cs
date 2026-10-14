using EngineService.Application.Mappers;
using FsCheck;
using FsCheck.Xunit;

namespace EngineService.Tests.PropertyBased;

public class IterationPreviewMapperPropertyTests
{
    private const int PodiumSize = 3;

    [Property]
    public bool Preview_ContainsOnlyPodium_OrderedByRank(int seed, PositiveInt teamCount)
    {
        var f = EngineFakers.Create(seed);
        int teams = teamCount.Get % 24 + 1;
        var scoreboard = EngineFakers.Scoreboard(f, teams);

        var previews = IterationPreviewMapper.GetIterationPreviewDtosAsync(scoreboard, EngineFakers.IterationResult(f));

        return previews.Select(p => p.Rank)
            .SequenceEqual(Enumerable.Range(1, Math.Min(PodiumSize, teams)));
    }

    [Property]
    public bool Preview_CarriesIterationAndTeamData(int seed, PositiveInt teamCount)
    {
        var f = EngineFakers.Create(seed);
        var scoreboard = EngineFakers.Scoreboard(f, teamCount.Get % 24 + 1);
        var iteration = EngineFakers.IterationResult(f);

        var previews = IterationPreviewMapper.GetIterationPreviewDtosAsync(scoreboard, iteration);

        return previews.All(p =>
        {
            var team = scoreboard.Single(t => t.Rank == p.Rank);
            return p.IterationId == iteration.Id
                && p.IterationIndex == iteration.IterationIndex
                && p.ScoreboardId == team.ScoreboardId
                && p.TeamId == team.TeamId
                && p.Points == team.Points;
        });
    }

    [Property]
    public bool Preview_IsEmpty_WithoutIteration(int seed, PositiveInt teamCount)
    {
        var f = EngineFakers.Create(seed);
        var scoreboard = EngineFakers.Scoreboard(f, teamCount.Get % 24 + 1);

        return IterationPreviewMapper.GetIterationPreviewDtosAsync(scoreboard, null!).Count == 0;
    }
}
