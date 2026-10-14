using Bogus;
using EngineService.Application.DTOs;

namespace EngineService.Tests.PropertyBased;

/// <summary>
/// Bogus-based builders of realistic test data.
/// FsCheck generates the seed, Bogus turns it into objects - every failing case is reproducible.
/// </summary>
internal static class EngineFakers
{
    public static Faker Create(int seed) => new() { Random = new Randomizer(seed) };

    public static IterationResultDto IterationResult(Faker f) => new()
    {
        Id = f.Random.Guid(),
        SimulationId = f.Random.Guid(),
        IterationIndex = f.Random.Int(1, 10_000),
        StartDate = f.Date.Recent(),
        ExecutionTime = TimeSpan.FromMilliseconds(f.Random.Int(10, 5_000)),
        TeamStrengths = new(),
        SimulatedMatchRounds = new()
    };

    /// <summary>
    /// Final league table of one iteration: <paramref name="teams"/> teams with unique ranks 1..N in random order.
    /// </summary>
    public static List<ScoreboardTeamStatsDto> Scoreboard(Faker f, int teams)
    {
        var scoreboardId = f.Random.Guid();
        var ranks = f.Random.Shuffle(Enumerable.Range(1, teams)).ToList();

        return ranks.Select(rank =>
        {
            int wins = f.Random.Int(0, 25);
            int draws = f.Random.Int(0, 10);
            int losses = f.Random.Int(0, 25);
            return new ScoreboardTeamStatsDto
            {
                Id = f.Random.Guid(),
                ScoreboardId = scoreboardId,
                TeamId = f.Random.Guid(),
                Rank = rank,
                Points = wins * 3 + draws,
                MatchPlayed = wins + draws + losses,
                Wins = wins,
                Draws = draws,
                Losses = losses,
                GoalsFor = f.Random.Int(10, 90),
                GoalsAgainst = f.Random.Int(10, 90)
            };
        }).ToList();
    }
}
