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
        var team = new ScoreboardTeamStats(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, 3, 1, 1, 0, 0, 2, 1);
        var other = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 1, 1, 0, 0, 1, 1, 1);

        // Act
        team.MergeMatchStats(other);

        // Assert
        Assert.Equal(4, team.Points);
        Assert.Equal(2, team.MatchPlayed);
        Assert.Equal(1, team.Wins);
        Assert.Equal(0, team.Losses);
        Assert.Equal(1, team.Draws);
        Assert.Equal(3, team.GoalsFor);
        Assert.Equal(2, team.GoalsAgainst);
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
        var stats1 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 3, 1, 1, 0, 0, 2, 1);
        var stats2 = new ScoreboardTeamStats(Guid.NewGuid(), team.ScoreboardId, team.TeamId, 0, 1, 1, 0, 1, 0, 0, 2);

        team.MergeMatchStats(stats1);
        team.MergeMatchStats(stats2);

        Assert.Equal(4, team.Points);
        Assert.Equal(2, team.MatchPlayed);
        Assert.Equal(1, team.Wins);
        Assert.Equal(1, team.Losses);
        Assert.Equal(0, team.Draws);
        Assert.Equal(2, team.GoalsFor);
        Assert.Equal(3, team.GoalsAgainst);
    }

}
