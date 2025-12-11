using System;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Tests.ClassTests;

public class TeamStrengthStartOfSeasonTests
{
    private const float DefaultLeagueStrength = 2.5f;
    private const float HomeAdvantage = 1.05f;
    private const int DefaultGamesToReachTrust = 10;
    private const float DefaultConfidenceLevel = 0.95f;

    private SimulationParams CreateDefaultSimulationParams()
    {
        return new SimulationParams()
        {
            Seed = 1000,
            Iterations = 1000,
            HomeAdvantage = HomeAdvantage,
            NoiseFactor = 0.1f,
            GamesToReachTrust = DefaultGamesToReachTrust,
            ConfidenceLevel = DefaultConfidenceLevel,
            SeasonYears = new List<string> { "2024/2025" },
            LeagueId = Guid.NewGuid()
        };
    }

    /// <summary>
    /// Test 1: Weryfikacja, że TeamStrength można stworzyć dla drużyny na start sezonu.
    /// </summary>
    [Fact]
    public void Create_ShouldCreateTeamStrengthWithZeroMatches()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var seasonEnum = SeasonEnum.Season2024_2025;
        var leagueId = Guid.NewGuid();

        // Act
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), teamId, seasonEnum, leagueId, DefaultLeagueStrength);

        // Assert
        Assert.NotNull(teamStrength);
        Assert.Equal(teamId, teamStrength.TeamId);
        Assert.Equal(0, teamStrength.SeasonStats.MatchesPlayed);
        Assert.Equal(0, teamStrength.SeasonStats.GoalsFor);
        Assert.Equal(0, teamStrength.SeasonStats.GoalsAgainst);
        Assert.Equal(0, teamStrength.Likelihood.Offensive);
        Assert.Equal(0, teamStrength.Likelihood.Defensive);
        Assert.Equal(0, teamStrength.Posterior.Offensive);
        Assert.Equal(0, teamStrength.Posterior.Defensive);
        Assert.Equal(0, teamStrength.ExpectedGoals);
    }

    /// <summary>
    /// Test 2: Weryfikacja, że WithPosterior na start sezonu daje wartości równe priorowi (league strength).
    /// </summary>
    [Fact]
    public void WithPosterior_OnStartOfSeason_ShouldReturnPriorValues()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        var simulationParams = CreateDefaultSimulationParams();

        // Act
        var updated = teamStrength.WithPosterior(DefaultLeagueStrength, simulationParams);

        // Assert
        // Na start sezonu bez meczów: Posterior = leagueStrength (czysty prior)
        Assert.True(
            Math.Abs(updated.Posterior.Offensive - DefaultLeagueStrength) < 0.01f,
            $"Expected Posterior.Offensive ≈ {DefaultLeagueStrength}, got {updated.Posterior.Offensive}"
        );

        Assert.True(
            Math.Abs(updated.Posterior.Defensive - DefaultLeagueStrength) < 0.01f,
            $"Expected Posterior.Defensive ≈ {DefaultLeagueStrength}, got {updated.Posterior.Defensive}"
        );
    }

    /// <summary>
    /// Test 3: Weryfikacja, że WithLikelihood wyrzuca wyjątek dla 0 meczów (słusznie).
    /// </summary>
    [Fact]
    public void WithLikelihood_OnStartOfSeason_ShouldThrowException()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => teamStrength.WithLikelihood());
    }

    /// <summary>
    /// Test 4: Weryfikacja, że ExpectedGoals jest automatycznie ustawiany na Posterior.Offensive.
    /// </summary>
    [Fact]
    public void WithPosterior_ShouldAutomaticallySetExpectedGoals()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        var simulationParams = CreateDefaultSimulationParams();

        // Act
        var updated = teamStrength.WithPosterior(DefaultLeagueStrength, simulationParams);

        // Assert
        Assert.True(
            Math.Abs(updated.ExpectedGoals - updated.Posterior.Offensive) < 0.001f,
            $"ExpectedGoals should equal Posterior.Offensive. " +
            $"ExpectedGoals: {updated.ExpectedGoals}, Posterior.Offensive: {updated.Posterior.Offensive}"
        );
    }

    /// <summary>
    /// Test 5: Weryfikacja, że po grze obu drużyn dane się zmienią (Likelihood ≠ Posterior).
    /// </summary>
    [Fact]
    public void AfterFirstMatch_LikelihoodAndPosteriorShouldDiffer()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        var simulationParams = CreateDefaultSimulationParams();

        // Symuluj pierwszą grę: drużyna strzeliła 2 gole, straciła 1
        var updatedStats = teamStrength.SeasonStats with
        {
            MatchesPlayed = 1,
            GoalsFor = 2,
            GoalsAgainst = 1,
            Wins = 1,
            Draws = 0,
            Losses = 0
        };

        var teamAfterMatch = teamStrength
            .WithSeasonStats(updatedStats)
            .WithLikelihood()
            .WithPosterior(DefaultLeagueStrength, simulationParams);

        // Assert
        // Likelihood to średnia z faktycznie granych meczów
        Assert.Equal(2f, teamAfterMatch.Likelihood.Offensive); // 2 gole / 1 mecz
        Assert.Equal(1f, teamAfterMatch.Likelihood.Defensive);  // 1 gol stracony / 1 mecz

        // Posterior to średnia ważona (Prior + Likelihood)
        Assert.NotEqual(teamAfterMatch.Likelihood.Offensive, teamAfterMatch.Posterior.Offensive);
        Assert.NotEqual(teamAfterMatch.Likelihood.Defensive, teamAfterMatch.Posterior.Defensive);

        // Posterior powinien być bliżej Priora niż Likelihood (bo GamesToReachTrust = 10)
        var posteriorOffensiveDistance = Math.Abs(teamAfterMatch.Posterior.Offensive - DefaultLeagueStrength);
        var likelihoodOffensiveDistance = Math.Abs(teamAfterMatch.Likelihood.Offensive - DefaultLeagueStrength);

        Assert.True(
            posteriorOffensiveDistance < likelihoodOffensiveDistance,
            "Posterior should be closer to Prior (LeagueStrength) than Likelihood at the start of the season"
        );
    }

    /// <summary>
    /// Test 6: Weryfikacja poprawności wyliczenia Priora Bayesowskiego.
    /// </summary>
    [Fact]
    public void WithPosterior_CalculationShouldFollowBayesianFormula()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );
        var simulationParams = new SimulationParams()
        {
            Seed = 1000,
            Iterations = 1000,
            HomeAdvantage = 1.05f,
            NoiseFactor = 0.1f,
            GamesToReachTrust = 10,
            ConfidenceLevel = 0.95f,
            SeasonYears = new List<string> { "2024/2025" },
            LeagueId = Guid.NewGuid()
        };

        // Act
        var updated = teamStrength.WithPosterior(DefaultLeagueStrength, simulationParams);

        // Ręczne obliczenie Priora Bayesowskiego dla weryfikacji
        // beta_0 = (1 - GamesToReachTrust) / GamesToReachTrust * ConfidenceLevel
        float expectedBeta_0 = simulationParams.ConfidenceLevel / simulationParams.GamesToReachTrust;

        float expectedAlpha0 = expectedBeta_0 * DefaultLeagueStrength;

        // Dla 0 meczów: Posterior = alpha_0 / beta_0 = expectedAlpha0 / expectedBeta0
        float expectedPosterior = expectedAlpha0 / expectedBeta_0; // = leagueStrength

        // Assert
        Assert.True(
            Math.Abs(updated.Posterior.Offensive - expectedPosterior) < 0.001f,
            $"Posterior calculation error. Expected: {expectedPosterior}, Got: {updated.Posterior.Offensive}"
        );
    }

    /// <summary>
    /// Test 7: Weryfikacja DeepClone na start sezonu.
    /// </summary>
    [Fact]
    public void DeepClone_ShouldCreateIndependentCopy()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        var simulationParams = CreateDefaultSimulationParams();
        var original = teamStrength.WithPosterior(DefaultLeagueStrength, simulationParams);

        // Act
        var cloned = original.DeepClone();

        // Assert
        Assert.Equal(original.TeamId, cloned.TeamId);
        Assert.Equal(original.Posterior.Offensive, cloned.Posterior.Offensive);
        Assert.Equal(original.ExpectedGoals, cloned.ExpectedGoals);

        // Modyfikacja klona nie powinna wpłynąć na oryginał
        var modifiedClone = cloned.UpdateTime();
        Assert.NotEqual(original.LastUpdate, modifiedClone.LastUpdate);
    }

    /// <summary>
    /// Test 8: Weryfikacja, że wszystkie drużyny na start mają podobne Posterior (równe LeagueStrength).
    /// </summary>
    [Theory]
    [InlineData(2.0f)]
    [InlineData(2.5f)]
    [InlineData(3.0f)]
    public void MultipleTeams_OnStartOfSeason_ShouldHaveSamePosterior(float leagueStrength)
    {
        // Arrange
        var team1 = TeamStrength.Create(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2024_2025, Guid.NewGuid(), leagueStrength);
        var team2 = TeamStrength.Create(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2024_2025, Guid.NewGuid(), leagueStrength);
        var team3 = TeamStrength.Create(Guid.NewGuid(), Guid.NewGuid(), SeasonEnum.Season2024_2025, Guid.NewGuid(), leagueStrength);

        var simulationParams = CreateDefaultSimulationParams();

        // Act
        var updated1 = team1.WithPosterior(leagueStrength, simulationParams);
        var updated2 = team2.WithPosterior(leagueStrength, simulationParams);
        var updated3 = team3.WithPosterior(leagueStrength, simulationParams);

        // Assert
        Assert.True(
            Math.Abs(updated1.Posterior.Offensive - updated2.Posterior.Offensive) < 0.001f
        );
        Assert.True(
            Math.Abs(updated2.Posterior.Offensive - updated3.Posterior.Offensive) < 0.001f
        );
    }

    /// <summary>
    /// Test 9: Weryfikacja SetRoundId na start sezonu.
    /// </summary>
    [Fact]
    public void SetRoundId_ShouldUpdateRoundIdForStartOfSeason()
    {
        // Arrange
        var teamStrength = TeamStrength.Create(Guid.NewGuid(), 
            Guid.NewGuid(),
            SeasonEnum.Season2024_2025,
            Guid.NewGuid(),
            DefaultLeagueStrength
        );

        var roundId = Guid.NewGuid();

        // Act
        var updated = teamStrength.SetRoundId(roundId);

        // Assert
        Assert.Equal(roundId, updated.RoundId);
    }

    /// <summary>
    /// Test 10: Integracyjny test - pełny flow od startu do kilku meczów.
    /// </summary>
    [Fact]
    public void FullSeasonFlow_ShouldShowProgressionFromPriorToPosteriorToLikelihood()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        var simulationParams = CreateDefaultSimulationParams();

        // Start sezonu
        var teamAtStart = TeamStrength.Create(Guid.NewGuid(), teamId, SeasonEnum.Season2024_2025, leagueId, DefaultLeagueStrength);
        var teamAfterStart = teamAtStart.WithPosterior(DefaultLeagueStrength, simulationParams);

        // Po meczu 1: wynik 3-1
        var stats1 = teamAfterStart.SeasonStats with
        {
            MatchesPlayed = 1,
            GoalsFor = 3,
            GoalsAgainst = 1,
            Wins = 1,
            Draws = 0,
            Losses = 0
        };
        var teamAfter1Match = teamAfterStart
            .WithSeasonStats(stats1)
            .WithLikelihood()
            .WithPosterior(DefaultLeagueStrength, simulationParams);

        // Po meczu 2: wynik 1-2
        var stats2 = teamAfter1Match.SeasonStats with
        {
            MatchesPlayed = 2,
            GoalsFor = 4,
            GoalsAgainst = 3,
            Wins = 1,
            Draws = 0,
            Losses = 1
        };
        var teamAfter2Matches = teamAfter1Match
            .WithSeasonStats(stats2)
            .WithLikelihood()
            .WithPosterior(DefaultLeagueStrength, simulationParams);

        // Assert - progresja danych
        Assert.True(teamAfterStart.Posterior.Offensive > 0);
        Assert.True(teamAfter1Match.Likelihood.Offensive > 0);
        Assert.True(teamAfter1Match.Posterior.Offensive > 0);

        // Likelihood powinno odzwierciedlać rzeczywiste wyniki
        Assert.True(Math.Abs(teamAfter2Matches.Likelihood.Offensive - 2f) < 0.001f); // 4 gole / 2 mecze
        Assert.True(Math.Abs(teamAfter2Matches.Likelihood.Defensive - 1.5f) < 0.001f); // 3 gole / 2 mecze

        // Posterior powinno być między Priorem a Likelihood
        var priorOffensive = DefaultLeagueStrength;
        var likelihoodOffensive = teamAfter2Matches.Likelihood.Offensive;
        var posteriorOffensive = teamAfter2Matches.Posterior.Offensive;

        var minValue = Math.Min(priorOffensive, likelihoodOffensive);
        var maxValue = Math.Max(priorOffensive, likelihoodOffensive);

        Assert.True(posteriorOffensive >= minValue && posteriorOffensive <= maxValue,
            $"Posterior should be between Prior and Likelihood. " +
            $"Prior: {priorOffensive}, Likelihood: {likelihoodOffensive}, Posterior: {posteriorOffensive}");

    }
}