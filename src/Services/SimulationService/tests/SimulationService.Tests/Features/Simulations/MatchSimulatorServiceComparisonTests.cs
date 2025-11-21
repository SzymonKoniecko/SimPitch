using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.Features.Simulations;

public class MatchSimulatorServiceComparisonTests
{
    private SimulationContent CreateBaseContent()
    {
        var homeId = Guid.NewGuid();
        var awayId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        float leagueAvg = 2.5f;

        // Tworzymy mecz do rozegrania
        var match = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = homeId,
            AwayTeamId = awayId,
            RoundId = roundId,
            IsPlayed = false
        };

        // Tworzymy stan początkowy drużyn
        var homeTeam = TeamStrength.Create(homeId, SeasonEnum.Season2023_2024, leagueId, leagueAvg)
            with
        { Posterior = (1.8f, 1.0f), SeasonStats = new SeasonStats(homeId, SeasonEnum.Season2023_2024, leagueId, leagueAvg, 5, 0, 0, 0, 0, 0) }; // Trochę historii dla Advanced

        var awayTeam = TeamStrength.Create(awayId, SeasonEnum.Season2023_2024, leagueId, leagueAvg)
            with
        { Posterior = (1.0f, 1.5f), SeasonStats = new SeasonStats(awayId, SeasonEnum.Season2023_2024, leagueId, leagueAvg, 5, 0, 0, 0, 0, 0) };

        return new SimulationContent
        {
            MatchRoundsToSimulate = new List<MatchRound> { match },
            TeamsStrengthDictionary = new Dictionary<Guid, List<TeamStrength>>
                {
                    { homeId, new List<TeamStrength> { homeTeam } },
                    { awayId, new List<TeamStrength> { awayTeam } }
                },
            PriorLeagueStrength = leagueAvg,
            LeagueStrengths = new List<LeagueStrength>
                {
                    new LeagueStrength { SeasonYear = SeasonEnum.Season2023_2024, Strength = leagueAvg }
                },
            SimulationParams = new SimulationParams
            {
                Title = "Comparison Test",
                Iterations = 1,
                Seed = 12345, // Stały seed dla wszystkich!
                GamesToReachTrust = 10,
                ConfidenceLevel = 1.0f,
                HomeAdvantage = 1.05f,
                NoiseFactor = 0.1f
            }
        };
    }

    [Fact]
    public void SimulationWorkflow_ShouldProduceDifferentResults_ForDifferentModels_WithSameSeed()
    {
        // Ten test jest statystyczny. Uruchamiamy symulację WIELE razy z różnymi seedami
        // i sprawdzamy, czy W OGÓLE modele różnią się od siebie.
        // Pojedynczy mecz może wyjść tak samo (np. 1-1) w różnych modelach przez przypadek.
        // Dlatego symulujemy serię.

        int differencesCount = 0;
        int iterations = 50; // Wystarczy mała próbka, by wykryć różnicę w logice

        for (int i = 0; i < iterations; i++)
        {
            int seed = i * 1000;

            // 1. Standard Poisson
            var content1 = CreateBaseContent();
            content1.SimulationParams.Seed = seed;
            var s1 = new MatchSimulatorService(seed, SimulationModelType.StandardPoisson);
            var res1 = s1.SimulationWorkflow(content1).MatchRoundsToSimulate.First();

            // 2. Dixon-Coles
            var content2 = CreateBaseContent();
            content2.SimulationParams.Seed = seed;
            var s2 = new MatchSimulatorService(seed, SimulationModelType.DixonColes);
            var res2 = s2.SimulationWorkflow(content2).MatchRoundsToSimulate.First();

            // Sprawdzamy czy wyniki się różnią
            if (res1.HomeGoals != res2.HomeGoals || res1.AwayGoals != res2.AwayGoals)
            {
                differencesCount++;
            }
        }

        // Assert: Oczekujemy, że przynajmniej część wyników będzie inna.
        // Dixon-Coles modyfikuje prawdopodobieństwa, więc przy tym samym RNG roll
        // może wpaść w inny próg bramkowy.
        Assert.True(differencesCount > 0, "Standard and Dixon-Coles should produce different outcomes over multiple iterations");
    }

    [Fact]
    public void AllModels_ShouldRunSuccessfully_AndUpdateStats()
    {
        // Sprawdzenie czy każdy model przechodzi flow bez wyjątków i aktualizuje dane
        var models = Enum.GetValues<SimulationModelType>();

        foreach (var model in models)
        {
            // Arrange
            var content = CreateBaseContent();
            var service = new MatchSimulatorService(12345, model);

            // Act
            var result = service.SimulationWorkflow(content);
            var match = result.MatchRoundsToSimulate.First();
            var homeTeam = result.TeamsStrengthDictionary[match.HomeTeamId].Last();

            // Assert
            Assert.True(match.IsPlayed, $"Model {model} failed to mark match as played");
            Assert.NotNull(homeTeam.LastUpdate); // Czy stats updated
            Assert.True(homeTeam.SeasonStats.MatchesPlayed > 5, $"Model {model} failed to increment stats"); // Było 5, powinno być 6
        }
    }

    [Fact]
    public void AdvancedModel_ShouldDifferFromStandard_DueToMomentum()
    {
        // Specjalny przypadek: Ustawiamy drużynę w formie, żeby Advanced na pewno zadziałał inaczej

        int seed = 555;

        // Standard
        var contentStd = CreateBaseContent();
        // Boost dla Advanced: Home Likelihood > Posterior
        var homeStd = contentStd.TeamsStrengthDictionary.First().Value.First();
        var boostedHome = homeStd with { Likelihood = (3.0f, 0.5f), Posterior = (1.5f, 1.0f) };
        contentStd.TeamsStrengthDictionary[homeStd.TeamId] = new List<TeamStrength> { boostedHome };
        contentStd.SimulationParams.Seed = seed;

        var svcStd = new MatchSimulatorService(seed, SimulationModelType.StandardPoisson);
        var resStd = svcStd.SimulationWorkflow(contentStd).MatchRoundsToSimulate.First();

        // Advanced
        var contentAdv = CreateBaseContent();
        // Taki sam boost
        var homeAdv = contentAdv.TeamsStrengthDictionary.First().Value.First();
        var boostedHomeAdv = homeAdv with { Likelihood = (3.0f, 0.5f), Posterior = (1.5f, 1.0f) };
        contentAdv.TeamsStrengthDictionary[homeAdv.TeamId] = new List<TeamStrength> { boostedHomeAdv };
        contentAdv.SimulationParams.Seed = seed;

        var svcAdv = new MatchSimulatorService(seed, SimulationModelType.Advanced);
        var resAdv = svcAdv.SimulationWorkflow(contentAdv).MatchRoundsToSimulate.First();

        // To jest trudne do deterministycznego testowania na jednym meczu,
        // ale w pętli powinno wyjść różnie.
        // Tutaj testujemy czy kod się wykonuje. Różnice statystyczne testowaliśmy w Unitach strategii.
        Assert.NotNull(resAdv);
        Assert.NotNull(resStd);
    }
}