using System;
using System.Collections.Generic;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;
using Xunit;

namespace StatisticsService.Tests;
public class ScoreboardServiceTests
{
    [Fact]
    public void CalculateSingleScoreboard_ShouldReturnScoreboardWithTeams()
    {
        // Arrange
        var service = new ScoreboardService(new ScoreboardTeamStatsService());

        var IterationResult = new IterationResult
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            SimulatedMatchRounds = new List<MatchRound>
            {
                new MatchRound
                {
                    Id = Guid.NewGuid(),
                    HomeTeamId = Guid.NewGuid(),
                    AwayTeamId = Guid.NewGuid(),
                    HomeGoals = 2,
                    AwayGoals = 1,
                    IsDraw = false
                }
            }
        };

        var playedRounds = new List<MatchRound>();

        // Act
        var result = service.CalculateSingleScoreboard(IterationResult, playedRounds);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.ScoreboardTeams);
    }
}
