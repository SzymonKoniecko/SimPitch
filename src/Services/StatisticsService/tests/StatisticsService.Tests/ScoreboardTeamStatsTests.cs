using System;
using StatisticsService.Domain.Entities;
using Xunit;

namespace StatisticsService.Tests;

public class ScoreboardTeamStatsTests
{
    [Fact]
    public void MergeMatchStats_ShouldAccumulateValues()
    {
        // Arrange
        // Mecz 1: Wygrana (3 pkt)
        var team = new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 3, 1, 1, 0, 0, 2, 1);
        // Mecz 2: Remis (1 pkt)
        var other = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 1, 1, 0, 0, 1, 1, 1);

        // Act
        team.MergeMatchStats(other);

        // Assert
        Assert.Equal(4, team.Points);        // 3 + 1
        Assert.Equal(2, team.MatchPlayed);   // 1 + 1
        Assert.Equal(1, team.Wins);          // 1 + 0
        Assert.Equal(0, team.Losses);        // 0 + 0
        Assert.Equal(1, team.Draws);         // 0 + 1  <-- Ważne!
        Assert.Equal(3, team.GoalsFor);      // 2 + 1
        Assert.Equal(2, team.GoalsAgainst);  // 1 + 1
    }

    [Fact]
    public void SetRanking_ShouldUpdateRank()
    {
        var team = new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 0, 0, 0, 0, 0, 0, 0);
        team.SetRanking(5);

        Assert.Equal(5, team.Rank);
    }

    [Fact]
    public void MergeMatchStats_ShouldAccumulateMultipleStats()
    {
        var team = new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 0, 0, 0, 0, 0, 0, 0);
        var stats1 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 3, 1, 1, 0, 0, 2, 1); // Wygrana
        var stats2 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 0, 1, 0, 1, 0, 0, 2); // Porażka

        team.MergeMatchStats(stats1);
        team.MergeMatchStats(stats2);

        Assert.Equal(3, team.Points);
        Assert.Equal(2, team.MatchPlayed);
        Assert.Equal(1, team.Wins);
        Assert.Equal(1, team.Losses);
        Assert.Equal(0, team.Draws);
        Assert.Equal(2, team.GoalsFor);
        Assert.Equal(3, team.GoalsAgainst);
    }

    [Fact]
    public void MergeMatchStats_DrawsShouldBeAggregatedCorrectly()
    {
        // Ten test weryfikuje scenariusz, który był problematyczny w HTML (błędne zliczanie remisów)

        // Arrange
        // Stan początkowy: 0 remisów
        var team = new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 0, 0, 0, 0, 0, 0, 0);

        // Mecz 1: Remis (1 pkt)
        var draw1 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 1, 1, 0, 0, 1, 1, 1);

        // Mecz 2: Remis (1 pkt)
        var draw2 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 1, 1, 0, 0, 1, 1, 1);

        // Act
        team.MergeMatchStats(draw1);
        team.MergeMatchStats(draw2);

        // Assert
        Assert.Equal(2, team.Points);       // 1 + 1
        Assert.Equal(2, team.MatchPlayed);  // 1 + 1
        Assert.Equal(0, team.Wins);
        Assert.Equal(0, team.Losses);
        Assert.Equal(2, team.Draws);        // Musi być 2!
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStatsAreInconsistent()
    {
        // Testuje walidację w konstruktorze (czy suma meczów się zgadza)

        Assert.Throws<ArgumentException>(() =>
            new ScoreboardTeamStats(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                rank: 0,
                points: 0,
                matchPlayed: 5, // Mówimy, że 5 meczów...
                wins: 1,
                losses: 1,
                draws: 1,       // ...ale suma to 3 (1+1+1)
                goalsFor: 0,
                goalsAgainst: 0)
        );
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPointsAreIncorrect()
    {
        // Testuje walidację punktów (czy 3*W + 1*D == Points)

        Assert.Throws<ArgumentException>(() =>
            new ScoreboardTeamStats(
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                rank: 0,
                points: 10,     // Podajemy 10 punktów...
                matchPlayed: 3,
                wins: 3,        // ...ale 3 wygrane to 9 punktów!
                losses: 0,
                draws: 0,
                goalsFor: 0,
                goalsAgainst: 0)
        );
    }
}
