using System;
using Microsoft.Extensions.Logging;
using Moq;
using StatisticsService.Application.Features.SimulationStats.Services;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Tests;

public class SimulationStatsServiceTests
{
    private readonly SimulationStatsService _service;

    public SimulationStatsServiceTests()
    {
        var loggerMock = new Mock<ILogger<SimulationStatsService>>();
        _service = new SimulationStatsService(loggerMock.Object);
    }

    [Fact]
    public void CalculateSimulationStatsForTeams_WhenSingleScoreboardTeamStatsPerTeam_ShouldCalculateProperly()
    {
        // Arrange
        var team1Id = Guid.NewGuid();
        var team2Id = Guid.NewGuid();

        var scoreboardStats = new List<ScoreboardTeamStats>
        {
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 1, points: 10, matchPlayed: 5, wins: 3, losses: 1, draws: 1, goalsFor: 8, goalsAgainst: 4),
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 2, points: 8, matchPlayed: 5, wins: 2, losses: 2, draws: 1, goalsFor: 6, goalsAgainst: 5)
        };

        // Act
        var result = _service.CalculateSimulationStatsForTeams(scoreboardStats, Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var team1Stats = result.First(x => x.TeamId == team1Id);
        var team2Stats = result.First(x => x.TeamId == team2Id);

        // Każda drużyna ma tylko jeden wynik, więc średnie == wartości wejściowe
        Assert.Equal(10, team1Stats.AverangePoints);
        Assert.Equal(8, team2Stats.AverangePoints);

        // Pozycje powinny mieć prawdopodobieństwo = 1 dla swojej pozycji
        Assert.Equal(1f, team1Stats.PositionProbbility[0]);
        Assert.Equal(1f, team2Stats.PositionProbbility[1]);
    }

    [Fact]
    public void CalculateSimulationStatsForTeams_WhenMultipleScoreboardTeamStatsPerTeam_ShouldAggregateAverages()
    {
        // Arrange
        var team1Id = Guid.NewGuid();
        var team2Id = Guid.NewGuid();

        var scoreboardStats = new List<ScoreboardTeamStats>
        {
            // Team 1 występuje dwa razy
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 1, points: 9, matchPlayed: 5, wins: 3, losses: 1, draws: 1, goalsFor: 10, goalsAgainst: 6),
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 2, points: 7, matchPlayed: 5, wins: 2, losses: 2, draws: 1, goalsFor: 7, goalsAgainst: 8),

            // Team 2 występuje dwa razy
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 2, points: 8, matchPlayed: 5, wins: 2, losses: 2, draws: 1, goalsFor: 9, goalsAgainst: 9),
            new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 1, points: 11, matchPlayed: 5, wins: 4, losses: 1, draws: 0, goalsFor: 12, goalsAgainst: 5)
        };

        // Act
        var result = _service.CalculateSimulationStatsForTeams(scoreboardStats, Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var team1Stats = result.First(x => x.TeamId == team1Id);
        var team2Stats = result.First(x => x.TeamId == team2Id);

        // Średnie punktów to (9 + 7) / 2 = 8
        Assert.Equal(8, Math.Round(team1Stats.AverangePoints, 2));
        Assert.Equal(9.5, Math.Round(team2Stats.AverangePoints, 2));

        // Każdy zespół był w rankach 1 i 2 po jednym razie, więc sumy powinny wynosić 1f w każdej pozycji
        Assert.Equal(0.5, team1Stats.PositionProbbility[0]);
        Assert.Equal(0.5, team1Stats.PositionProbbility[1]);

        Assert.Equal(0.5, team2Stats.PositionProbbility[0]);
        Assert.Equal(0.5, team2Stats.PositionProbbility[1]);
    }

    [Fact]
    public void SetNormalizedPositionProbability_ShouldReturnCorrectNormalizedPercentages()
    {
        // Arrange
        var simulationId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var stats = new SimulationTeamStats(simulationId, teamId, 3);

        // Dodajemy cztery symulacje z różnymi pozycjami:
        stats.AddFromScoreboardStats(0, 10, 1, 0, 0, 5, 2); // 1. miejsce
        stats.AddFromScoreboardStats(1, 7, 0, 1, 0, 3, 1);  // 2. miejsce
        stats.AddFromScoreboardStats(1, 8, 0, 1, 0, 4, 3);  // 2. miejsce
        stats.AddFromScoreboardStats(2, 5, 0, 2, 0, 2, 5);  // 3. miejsce

        // Act
        var normalized = stats.SetNormalizedPositionProbability();

        // Assert
        Assert.NotNull(normalized);
        Assert.Equal(3, normalized.Length);

        // Pozycje: [1x 1st, 2x 2nd, 1x 3rd] -> razem 4 symulacje
        Assert.Equal(0.25f, normalized[0], 3);
        Assert.Equal(0.5f, normalized[1], 3);
        Assert.Equal(0.25f, normalized[2], 3);

        // Suma powinna być ≈ 1.0
        var total = normalized.Sum();
        Assert.InRange(total, 0.999f, 1.001f);
    }

    [Fact]
    public void SetNormalizedPositionProbability_ShouldEnsureTotalProbabilityEqualsOne()
    {
        // Arrange
        var simulationId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var stats = new SimulationTeamStats(simulationId, teamId, 5);

        // symulujemy 10 wyników z różnymi pozycjami
        stats.AddFromScoreboardStats(0, 10, 1, 0, 0, 5, 2); // 1st place
        stats.AddFromScoreboardStats(1, 9, 1, 0, 0, 4, 2);
        stats.AddFromScoreboardStats(1, 8, 0, 1, 0, 3, 1);
        stats.AddFromScoreboardStats(2, 7, 0, 1, 0, 2, 1);
        stats.AddFromScoreboardStats(2, 7, 0, 1, 0, 2, 1);
        stats.AddFromScoreboardStats(3, 6, 0, 2, 0, 1, 4);
        stats.AddFromScoreboardStats(3, 6, 0, 2, 0, 1, 4);
        stats.AddFromScoreboardStats(4, 5, 0, 2, 0, 1, 5);
        stats.AddFromScoreboardStats(4, 5, 0, 2, 0, 1, 5);
        stats.AddFromScoreboardStats(4, 5, 0, 2, 0, 1, 5);

        // Act
        var normalized = stats.SetNormalizedPositionProbability();
        var total = normalized.Sum();

        // Assert
        // suma wszystkich pozycji musi wynosić dokładnie 1.0 (±mała tolerancja)
        Assert.InRange(total, 0.999f, 1.001f);

        // dodatkowo: każda wartość musi być między 0 a 1
        foreach (var p in normalized)
            Assert.InRange(p, 0f, 1f);
    }

    [Fact]
    public void CalculateSimulationStatsForTeams_ShouldProduceNormalizedProbabilitiesSummingToOne()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<SimulationStatsService>>();
        var service = new SimulationStatsService(loggerMock.Object);

        var team1Id = Guid.NewGuid();
        var team2Id = Guid.NewGuid();
        var team3Id = Guid.NewGuid();

        // symulujemy 3 drużyny grające kilka "turniejów" z różnymi miejscami
        var scoreboardStats = new List<ScoreboardTeamStats>
    {
        // TEAM 1 — 3 różne pozycje
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 1, points: 10, matchPlayed: 5, wins: 3, losses: 1, draws: 1, goalsFor: 8, goalsAgainst: 4),
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 2, points: 8, matchPlayed: 5, wins: 2, losses: 2, draws: 1, goalsFor: 7, goalsAgainst: 6),
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team1Id, rank: 3, points: 6, matchPlayed: 5, wins: 1, losses: 3, draws: 1, goalsFor: 5, goalsAgainst: 7),

        // TEAM 2 — częściej wygrywa
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 1, points: 12, matchPlayed: 5, wins: 4, losses: 1, draws: 0, goalsFor: 10, goalsAgainst: 3),
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 1, points: 11, matchPlayed: 5, wins: 3, losses: 1, draws: 1, goalsFor: 9, goalsAgainst: 4),
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team2Id, rank: 2, points: 9, matchPlayed: 5, wins: 2, losses: 2, draws: 1, goalsFor: 8, goalsAgainst: 5),

        // TEAM 3 — zawsze ostatni
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team3Id, rank: 3, points: 4, matchPlayed: 5, wins: 1, losses: 4, draws: 0, goalsFor: 3, goalsAgainst: 9),
        new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), team3Id, rank: 3, points: 3, matchPlayed: 5, wins: 0, losses: 5, draws: 0, goalsFor: 2, goalsAgainst: 10)
    };

        // Act
        var result = service.CalculateSimulationStatsForTeams(scoreboardStats, Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // 3 drużyny

        foreach (var teamStats in result)
        {
            var total = teamStats.PositionProbbility.Sum();

            // suma musi wynosić ≈ 1.0
            Assert.InRange(total, 0.999f, 1.001f);

            // każda wartość w poprawnym zakresie [0, 1]
            foreach (var p in teamStats.PositionProbbility)
                Assert.InRange(p, 0f, 1f);
        }
    }
}
