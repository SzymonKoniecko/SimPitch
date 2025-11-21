using System;
using SimulationService.Domain.Services.SimulationModels;

namespace SimulationService.Tests.Features.Simulations.SimulationModels;

public class DixonColesStrategyTests : StrategyTestBase
{
    [Fact]
    public void SimulateMatch_ShouldReturnValidScore_UsingTauCorrection()
    {
        // Arrange
        var strategy = new DixonColesStrategy();

        // Act
        var (h, a) = strategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, DeterministicRng);

        // Assert
        Assert.True(h >= 0);
        Assert.True(a >= 0);
    }

    [Fact]
    public void SimulateMatch_ShouldProduceDifferentDistribution_ThanStandard()
    {
        // Arrange
        var dcStrategy = new DixonColesStrategy();
        var stdStrategy = new StandardPoissonStrategy();
        int iterations = 2000;

        int dcDraws00 = 0;
        int stdDraws00 = 0;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var r1 = new Random(i);
            var (h1, a1) = dcStrategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, r1);
            if (h1 == 0 && a1 == 0) dcDraws00++;

            var r2 = new Random(i);
            var (h2, a2) = stdStrategy.SimulateMatch(HomeTeam, AwayTeam, LeagueAvgGoals, SimParams, r2);
            if (h2 == 0 && a2 == 0) stdDraws00++;
        }

        // Assert
        // Dixon-Coles modyfikuje prawdopodobieństwo 0-0, więc liczba takich wyników musi być inna
        Assert.NotEqual(dcDraws00, stdDraws00);
    }
}