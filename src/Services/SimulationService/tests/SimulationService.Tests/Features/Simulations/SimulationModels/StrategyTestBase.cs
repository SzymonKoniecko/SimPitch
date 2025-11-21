using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;
using SimulationService.Domain.Enums;
using Xunit;
using System;

namespace SimulationService.Tests.Features.Simulations.SimulationModels;

public abstract class StrategyTestBase
{
    protected readonly TeamStrength HomeTeam;
    protected readonly TeamStrength AwayTeam;
    protected readonly SimulationParams SimParams;
    protected readonly Random DeterministicRng;
    protected readonly float LeagueAvgGoals = 2.5f; // Średnia bramek w meczu (1.25 na drużynę)

    protected StrategyTestBase()
    {
        var homeId = Guid.NewGuid();
        var awayId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();

        // Tworzymy bazowe obiekty
        var baseHome = TeamStrength.Create(homeId, SeasonEnum.Season2023_2024, leagueId, LeagueAvgGoals);
        var baseAway = TeamStrength.Create(awayId, SeasonEnum.Season2023_2024, leagueId, LeagueAvgGoals);

        // Konfigurujemy statystyki "na sztywno" dla testów
        // Posterior 1.5 oznacza atak o sile 1.5 gola/mecz (przy średniej przeciwnika)

        HomeTeam = baseHome with
        {
            Posterior = (1.5f, 1.0f), // Dobry atak, przeciętna obrona
            SeasonStats = baseHome.SeasonStats with { MatchesPlayed = 10 }
        };

        AwayTeam = baseAway with
        {
            Posterior = (1.0f, 1.5f), // Przeciętny atak, słaba obrona (puszcza 1.5)
            SeasonStats = baseAway.SeasonStats with { MatchesPlayed = 10 }
        };

        SimParams = new SimulationParams
        {
            Title = "Test Simulation",
            SeasonYears = new() { "2023/2024" },
            LeagueId = leagueId,
            Iterations = 1,
            Seed = 42, // Seed 42 dla determinizmu
            GamesToReachTrust = 10,
            ConfidenceLevel = 1.0f,
            HomeAdvantage = 1.05f,
            NoiseFactor = 0.1f
        };

        DeterministicRng = new Random(SimParams.Seed);
    }
}