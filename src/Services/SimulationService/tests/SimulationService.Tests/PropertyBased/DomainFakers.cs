using Bogus;
using SimulationService.Domain.Consts;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.PropertyBased;

/// <summary>
/// Bogus-based builders of realistic domain data.
/// FsCheck generates the seed, Bogus turns it into domain objects - every failing case is reproducible.
/// </summary>
internal static class DomainFakers
{
    public static Faker Create(int seed) => new() { Random = new Randomizer(seed) };

    public static SeasonStats SeasonStats(Faker f, Guid? teamId = null, int minMatches = 0)
    {
        int wins = f.Random.Int(0, 20);
        int draws = f.Random.Int(0, 15);
        int losses = f.Random.Int(0, 20);
        int matches = wins + draws + losses;
        if (matches < minMatches)
        {
            wins += minMatches - matches;
            matches = minMatches;
        }

        return new SeasonStats(
            f.Random.Guid(),
            teamId ?? f.Random.Guid(),
            f.PickRandom<SeasonEnum>(),
            f.Random.Guid(),
            leagueStrength: f.Random.Float(1.5f, 3.5f),
            matchesPlayed: matches,
            wins: wins,
            losses: losses,
            draws: draws,
            goalsFor: f.Random.Int(0, matches * 3),
            goalsAgainst: f.Random.Int(0, matches * 3));
    }

    public static MatchRound PlayedMatch(Faker f, Guid homeTeamId, Guid awayTeamId)
    {
        int homeGoals = f.Random.Int(0, 6);
        int awayGoals = f.Random.Int(0, 6);
        return new MatchRound
        {
            Id = f.Random.Guid(),
            RoundId = f.Random.Guid(),
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            HomeGoals = homeGoals,
            AwayGoals = awayGoals,
            IsDraw = homeGoals == awayGoals,
            IsPlayed = true
        };
    }

    public static MatchRound MatchToSimulate(Faker f, Guid homeTeamId, Guid awayTeamId) => new()
    {
        Id = f.Random.Guid(),
        RoundId = f.Random.Guid(),
        HomeTeamId = homeTeamId,
        AwayTeamId = awayTeamId,
        IsPlayed = false
    };

    public static SimulationParams SimulationParams(Faker f) => new()
    {
        Title = f.Lorem.Sentence(3),
        SeasonYears = new() { "2026/2027" },
        LeagueId = f.Random.Guid(),
        Iterations = 1,
        Seed = f.Random.Int(),
        GamesToReachTrust = f.Random.Int(1, 30),
        ConfidenceLevel = SimulationConsts.SIMULATION_CONFIDENCE_LEVEL,
        HomeAdvantage = f.Random.Float(1.0f, 1.2f),
        NoiseFactor = f.Random.Float(0f, 0.3f),
        ModelType = nameof(SimulationModelType.StandardPoisson)
    };

    /// <summary>
    /// Team strength the way the simulation prepares it: season stats -> likelihood -> posterior.
    /// </summary>
    public static TeamStrength TeamStrength(Faker f, Guid teamId, float leagueStrength, SimulationParams simulationParams)
    {
        var stats = SeasonStats(f, teamId, minMatches: 1);
        return global::SimulationService.Domain.ValueObjects.TeamStrength
            .Create(stats.Id, teamId, stats.SeasonYear, stats.LeagueId, stats.LeagueStrength)
            .WithSeasonStats(stats)
            .WithLikelihood()
            .WithPosterior(leagueStrength, simulationParams);
    }

    /// <summary>
    /// A league with 4-10 teams and a random subset of the round-robin fixtures to simulate.
    /// </summary>
    public static SimulationContent SimulationContent(Faker f)
    {
        var simulationParams = SimulationParams(f);
        float leagueStrength = f.Random.Float(2.0f, 3.2f);
        var teamIds = Enumerable.Range(0, f.Random.Int(4, 10)).Select(_ => f.Random.Guid()).ToList();

        var content = new SimulationContent
        {
            SimulationParams = simulationParams,
            PriorLeagueStrength = leagueStrength,
            LeagueStrengths = new()
            {
                new LeagueStrength { Id = f.Random.Guid(), LeagueId = simulationParams.LeagueId, SeasonYear = SimulationConsts.CURRENT_SEASON, Strength = leagueStrength }
            }
        };

        foreach (var teamId in teamIds)
        {
            content.TeamsStrengthDictionary[teamId] = new() { TeamStrength(f, teamId, leagueStrength, simulationParams) };
        }

        foreach (var home in teamIds)
        {
            foreach (var away in teamIds.Where(id => id != home))
            {
                if (f.Random.Bool(0.4f))
                    content.MatchRoundsToSimulate.Add(MatchToSimulate(f, home, away));
            }
        }

        return content;
    }
}
