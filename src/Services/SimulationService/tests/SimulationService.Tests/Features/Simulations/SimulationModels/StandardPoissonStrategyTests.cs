using System;
using SimulationService.Domain.Services.SimulationModels;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.Features.Simulations.SimulationModels;

public class StandardPoissonStrategyTests : StrategyTestBase
{
    [Fact]
    public void SimulateMatch_ShouldReturnDeterministicResult_WithFixedSeed()
    {
        // Arrange
        var strategy = new StandardPoissonStrategy();
        var rng1 = new Random(SimParams.Seed);
        var rng2 = new Random(SimParams.Seed);

        // Act
        // Przekazujemy LeagueAvgGoals (np. 2.5f)
        var (h1, a1) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, rng1);
        var (h2, a2) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, rng2);

        // Assert
        Assert.Equal(h1, h2);
        Assert.Equal(a1, a2);
    }

    [Fact]
    public void SimulateMatch_ShouldProduceReasonableScores()
    {
        // Arrange
        var strategy = new StandardPoissonStrategy();

        // Act
        var (h, a) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, DeterministicRng);

        // Assert
        // Przy średnich ataku 1.5 i 1.0 spodziewamy się wyników w normalnym zakresie piłkarskim
        Assert.InRange(h, 0, 10);
        Assert.InRange(a, 0, 10);
    }

    [Fact]
    public void SimulateMatch_ShouldFavorHomeTeam_WithHighAdvantage()
    {
        // Arrange
        var strategy = new StandardPoissonStrategy();
        // Kopia params z dużą przewagą
        var highAdvantageParams = new SimulationParams
        {
            HomeAdvantage = 2.0f,
            NoiseFactor = 0.1f
        };

        int homeWins = 0;
        int iterations = 1000;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var (h, a) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, highAdvantageParams, new Random(i));
            if (h > a) homeWins++;
        }

        // Assert
        Assert.True(homeWins > iterations * 0.6, "Home team should win > 60% with 2.0 advantage");
    }
}