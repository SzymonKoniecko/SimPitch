using System;
using StatisticsService.Domain.Entities;
using Xunit;

namespace StatisticsService.Tests;
public class ScoreboardTests
{
    [Fact]
    public void SetRankings_ShouldAssignRanksCorrectly()
    {
        // Arrange
        var scoreboard = new Scoreboard(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1.0f, 1.0f, DateTime.Now);

        scoreboard.AddTeam(new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 0, 10, 1, 3, 0, 0, 5, 2));
        scoreboard.AddTeam(new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 0, 8, 1, 2, 1, 0, 4, 3));

        // Act
        scoreboard.SetRankings();

        // Assert
        Assert.Equal(1, scoreboard.ScoreboardTeams.First().Rank);
        Assert.Equal(2, scoreboard.ScoreboardTeams.Last().Rank);
    }
    [Fact]
    public void SortByCriteria_ShouldPrioritizePointsOverWins()
    {
        var scoreboard = new Scoreboard(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, 1, DateTime.Now);
        var team1 = new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 0, 5, 1, 1, 0, 2, 3, 2);
        var team2 = new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 0, 8, 1, 2, 1, 0, 4, 3);

        scoreboard.AddTeam(team1);
        scoreboard.AddTeam(team2);

        scoreboard.SortByCriteria();

        Assert.Equal(team2, scoreboard.ScoreboardTeams.First());
    }

    [Fact]
    public void SortByRank_ShouldSortAscending()
    {
        var scoreboard = new Scoreboard(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, 1, DateTime.Now);
        var t1 = new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 2, 0, 0, 0, 0, 0, 0, 0);
        var t2 = new ScoreboardTeamStats(Guid.NewGuid(), scoreboard.Id, Guid.NewGuid(), 1, 0, 0, 0, 0, 0, 0, 0);

        scoreboard.AddTeam(t1);
        scoreboard.AddTeam(t2);

        scoreboard.SortByRank();

        Assert.Equal(t2, scoreboard.ScoreboardTeams.First());
    }

}
