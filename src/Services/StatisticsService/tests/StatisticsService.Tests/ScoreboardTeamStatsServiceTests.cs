using System;
using System.Collections.Generic;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;
using Xunit;

namespace StatisticsService.Tests;
public class ScoreboardTeamStatsServiceTests
{
    [Fact]
    public void CalculateScoreboardTeamStats_ShouldAggregateCorrectly()
    {
        // Arrange
        var service = new ScoreboardTeamStatsService();
        var scoreboardId = Guid.NewGuid();
        var oneTeamId = Guid.NewGuid();
        var secondTeamId = Guid.NewGuid();
        var matches = new List<MatchRound>
        {
            new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = oneTeamId,
                AwayTeamId = secondTeamId,
                HomeGoals = 2,
                AwayGoals = 1,
                IsDraw = false
            },
            new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = secondTeamId,
                AwayTeamId = oneTeamId,
                HomeGoals = 1,
                AwayGoals = 1,
                IsDraw = true
            },
            new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = secondTeamId,
                AwayTeamId = oneTeamId,
                HomeGoals = 10,
                AwayGoals = 1,
                IsDraw = true
            }
        };


        // Act
        var stats = service.CalculateScoreboardTeamStats(scoreboardId, matches);

        // Assert
        Assert.NotNull(stats);
        Assert.True(stats.Count > 0);
        Assert.Equal(4, stats.Where(x => x.TeamId == oneTeamId).Select(x => x.GoalsFor).Sum());
        Assert.Equal(12, stats.Where(x => x.TeamId == oneTeamId).Select(x => x.GoalsAgainst).Sum());
    }
    [Fact]
    public void CalculateScoreboardTeamStatsForMatch_ShouldAssignWinCorrectly()
    {
        var service = new ScoreboardTeamStatsService();
        var scoreboardId = Guid.NewGuid();

        var match = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = Guid.NewGuid(),
            AwayTeamId = Guid.NewGuid(),
            HomeGoals = 2,
            AwayGoals = 1,
            IsDraw = false
        };

        var (home, away) = service.CalculateScoreboardTeamStatsForMatch(scoreboardId, match);

        Assert.Equal(3, home.Points);
        Assert.Equal(0, away.Points);
        Assert.Equal(1, home.Wins);
        Assert.Equal(1, away.Losses);
        Assert.Equal(1, home.Wins);
        Assert.Equal(1, away.Losses);
    }

    [Fact]
    public void CalculateScoreboardTeamStatsForMatch_ShouldAssignDrawCorrectly()
    {
        var service = new ScoreboardTeamStatsService();
        var scoreboardId = Guid.NewGuid();

        var match = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = Guid.NewGuid(),
            AwayTeamId = Guid.NewGuid(),
            HomeGoals = 1,
            AwayGoals = 1,
            IsDraw = true
        };

        var (home, away) = service.CalculateScoreboardTeamStatsForMatch(scoreboardId, match);

        Assert.Equal(1, home.Points);
        Assert.Equal(1, away.Points);
        Assert.Equal(1, home.Draws);
        Assert.Equal(1, away.Draws);
    }

}
