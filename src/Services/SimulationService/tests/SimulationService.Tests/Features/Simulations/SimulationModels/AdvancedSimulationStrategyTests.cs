using System;
using SimulationService.Domain.Services.SimulationModels;

namespace SimulationService.Tests.Features.Simulations.SimulationModels;

public class AdvancedSimulationStrategyTests : StrategyTestBase
{
    [Fact]
    public void SimulateMatch_ShouldBoostTeamWithMomentum()
    {
        // Arrange
        var strategy = new AdvancedSimulationStrategy();

        // Konfiguracja "Hot Home Team" (Momentum)
        // Likelihood (3.0) > Posterior (1.5) => Momentum Boost
        // MatchesPlayed = 10 (spełnia warunek >= 3)
        var hotHomeTeam = HomeTeam with
        {
            Likelihood = (3.0f, 0.5f),
            Posterior = (1.5f, 1.0f),
            SeasonStats = HomeTeam.SeasonStats with { MatchesPlayed = 10 }
        };

        // Konfiguracja "Normal Away Team"
        var normalAwayTeam = AwayTeam with
        {
            Likelihood = (1.0f, 1.5f),
            Posterior = (1.0f, 1.5f),
            SeasonStats = AwayTeam.SeasonStats with { MatchesPlayed = 10 }
        };

        int homeWins = 0;
        int iterations = 2000;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var (h, a) = strategy.SimulateMatch(hotHomeTeam, normalAwayTeam, LeagueAvgGoals, SimParams, new Random(i));
            if (h > a) homeWins++;
        }

        double winRate = (double)homeWins / iterations;

        // Assert
        // LambdaHome ≈ (1.5 * 1.5 / 1.25) * 1.05 * 1.1(momentum) ≈ 2.07 gola
        // LambdaAway ≈ (1.0 * 1.0 / 1.25) ≈ 0.8 gola
        // Przewaga 2.07 vs 0.8 to pewne zwycięstwo w > 60% przypadków.
        Assert.True(homeWins > iterations * 0.5,
            $"Home team with momentum should win frequently. Actual win rate: {winRate:P1}");
    }

    [Fact]
    public void SimulateMatch_ShouldBeEquivalentToDixonColes_ForNewSeason()
    {
        // Arrange
        var advStrategy = new AdvancedSimulationStrategy();
        var dcStrategy = new DixonColesStrategy();

        // Nowy sezon -> MatchesPlayed = 0 -> Brak momentum
        var newHome = HomeTeam with { SeasonStats = HomeTeam.SeasonStats with { MatchesPlayed = 0 } };
        var newAway = AwayTeam with { SeasonStats = AwayTeam.SeasonStats with { MatchesPlayed = 0 } };

        var seed = 12345;

        // Act
        var advResult = advStrategy.SimulateMatch(newHome, newAway, LeagueAvgGoals, SimParams, new Random(seed));
        var dcResult = dcStrategy.SimulateMatch(newHome, newAway, LeagueAvgGoals, SimParams, new Random(seed));

        // Assert
        // Wyniki muszą być identyczne, bo Momentum nie działa dla MatchesPlayed < 3
        Assert.Equal(advResult, dcResult);
    }
}