using System;
using SimulationService.Domain.Services.SimulationModels;

namespace SimulationService.Tests.Features.Simulations.SimulationModels;

public class BivariatePoissonStrategyTests : StrategyTestBase
{
    [Fact]
    public void SimulateMatch_ShouldAllowCorrelatedHighScores()
    {
        // Arrange
        var strategy = new BivariatePoissonStrategy();
        int bothScore2Plus = 0;
        int iterations = 1000;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var (h, a) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, new Random(i));
            // Sprawdzamy efekt kowariancji (lambda3) - tendencję do wspólnych wysokich wyników
            if (h >= 2 && a >= 2) bothScore2Plus++;
        }

        // Assert
        Assert.True(bothScore2Plus > 0, "Should generate matches where both teams score multiple goals");
    }

    [Fact]
    public void SimulateMatch_ShouldRunWithoutExceptions()
    {
        // Podstawowy sanity check
        var strategy = new BivariatePoissonStrategy();
        var (h, a) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, DeterministicRng);
        Assert.True(h >= 0 && a >= 0);
    }
}