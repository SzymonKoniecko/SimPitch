using System;
using System.Collections.Generic;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;
using Xunit;

public class ScoreboardServiceTests
{
    [Fact]
    public void CalculateSingleScoreboard_ShouldReturnScoreboardWithTeams()
    {
        // Arrange
        var service = new ScoreboardService(new ScoreboardTeamStatsService());

        var simulationResult = new SimulationResult
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1.2f,
            PriorLeagueStrength = 1.1f,
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
        var result = service.CalculateSingleScoreboard(simulationResult, playedRounds);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.ScoreboardTeams);
    }
}
