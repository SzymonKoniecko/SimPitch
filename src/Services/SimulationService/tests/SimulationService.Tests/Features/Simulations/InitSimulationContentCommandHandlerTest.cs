using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;
using SimulationService.Domain.Consts;

namespace SimulationService.Tests.Application.Features.Simulations;

public class InitSimulationContentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCorrectContent_WithPriorStrengthAndTeams()
    {
        // Arrange
        var seasonStatsService = new SeasonStatsService();
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<InitSimulationContentCommandHandler>>();

        var homeTeamId = Guid.NewGuid();
        var awayTeamId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        var roundId = Guid.NewGuid();
        string currentSeasonStr = "2023/2024";
        SeasonEnum currentSeasonEnum = SeasonEnum.Season2023_2024;

        // Simulation Params DTO
        var paramsDto = new SimulationParamsDto
        {
            SeasonYears = new List<string> { currentSeasonStr },
            LeagueId = leagueId,
            LeagueRoundId = roundId,
            Seed = 1000,
            GamesToReachTrust = 10,
            ConfidenceLevel = 0.95f,
            HomeAdvantage = 1.05f,
            NoiseFactor = 0.1f
        };

        // Mock Data from DB
        var leagueRounds = new List<LeagueRound>
            {
                new LeagueRound { Id = roundId, LeagueId = leagueId, SeasonYear = currentSeasonStr }
            };

        var league = new League
        {
            Id = leagueId,
            LeagueStrengths = new List<LeagueStrength>
                {
                    new LeagueStrength { LeagueId = leagueId, SeasonYear = currentSeasonEnum, Strength = 1.8f },
                    new LeagueStrength { LeagueId = leagueId, SeasonYear = SeasonEnum.Season2022_2023, Strength = 1.8f }
                }
        };

        // One played match (2-1), one to simulate
        var matchRounds = new List<MatchRound>
            {
                new MatchRound { Id = Guid.NewGuid(), HomeTeamId = homeTeamId, AwayTeamId = awayTeamId, HomeGoals = 2, AwayGoals = 1, IsPlayed = true },
                new MatchRound { Id = Guid.NewGuid(), HomeTeamId = homeTeamId, AwayTeamId = awayTeamId, HomeGoals = 0, AwayGoals = 0, IsPlayed = false }
            };

        // Setup Mediator Calls
        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(leagueRounds);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(league);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchRounds);

        var handler = new InitSimulationContentCommandHandler(seasonStatsService, mediatorMock.Object, loggerMock.Object);
        var command = new InitSimulationContentCommand(paramsDto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Check PriorLeagueStrength calculation: (2+1) goals / 1 match = 3.0
        Assert.Equal(3.0f, result.PriorLeagueStrength);

        // Check LeagueStrength passed correctly
        Assert.Single(result.LeagueStrengths);
        Assert.Equal(1.8f, result.LeagueStrengths.First().Strength);

        // Check MatchRoundsToSimulate (only the unplayed one)
        Assert.Single(result.MatchRoundsToSimulate);
        Assert.False(result.MatchRoundsToSimulate[0].IsPlayed);

        // Check Teams dictionary populated
        Assert.True(result.TeamsStrengthDictionary.ContainsKey(homeTeamId));
        Assert.True(result.TeamsStrengthDictionary.ContainsKey(awayTeamId));

        // Verify Stats accumulation for HomeTeam (1 win, 2 goals for)
        var homeStats = result.TeamsStrengthDictionary[homeTeamId].First().SeasonStats;
        Assert.Equal(1, homeStats.MatchesPlayed);
        Assert.Equal(2, homeStats.GoalsFor);
    }

    [Fact]
    public async Task Handle_ShouldUseFallbackPriorStrength_WhenNoMatchesPlayed()
    {
        // Arrange
        var seasonStatsService = new SeasonStatsService();
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<InitSimulationContentCommandHandler>>();

        var leagueId = Guid.NewGuid();
        string currentSeasonStr = "2023/2024";

        var paramsDto = new SimulationParamsDto
        {
            SeasonYears = new List<string> { currentSeasonStr },
            LeagueId = leagueId,
            LeagueRoundId = Guid.NewGuid(),
            GamesToReachTrust = SimulationConsts.GAMES_TO_REACH_TRUST,
            NoiseFactor = SimulationConsts.NOISE_FACTOR,
            HomeAdvantage = SimulationConsts.HOME_ADVANTAGE
        };

        // Mock Data: No played matches
        var matchRounds = new List<MatchRound>
            {
                new MatchRound { Id = Guid.NewGuid(), IsPlayed = false }
            };

        var league = new League { Id = leagueId, LeagueStrengths = new List<LeagueStrength>(){new LeagueStrength() { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = SeasonEnum.Season2022_2023, Strength = 1.0f}} }; // No strength in DB

        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRound> { new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = currentSeasonStr } });
        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(league);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchRounds);

        var handler = new InitSimulationContentCommandHandler(seasonStatsService, mediatorMock.Object, loggerMock.Object);
        var command = new InitSimulationContentCommand(paramsDto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        // Should use fallback 2.5f because totalMatches == 0
        Assert.Equal(2.5f, result.PriorLeagueStrength);
    }

    [Fact]
    public async Task Handle_ShouldMergeHistoricalData_WhenMultipleSeasonsProvided()
    {
        // Arrange
        var seasonStatsService = new SeasonStatsService();
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<InitSimulationContentCommandHandler>>();

        var teamId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        string prevSeasonStr = "2022/2023";
        string currentSeasonStr = "2023/2024";

        // Requesting BOTH seasons
        var paramsDto = new SimulationParamsDto
        {
            SeasonYears = new List<string> { prevSeasonStr, currentSeasonStr },
            LeagueId = leagueId,
            LeagueRoundId = Guid.NewGuid(),
            GamesToReachTrust = SimulationConsts.GAMES_TO_REACH_TRUST
        };

        // Current Season Data (1 match played, 1 goal scored)
        var currentMatch = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = teamId,
            AwayTeamId = Guid.NewGuid(),
            HomeGoals = 1,
            AwayGoals = 0,
            IsPlayed = true
        };

        // Historical Data (Season 2022/23: 10 goals scored)
        // Note: Returning SeasonStats object directly as DTO
        var historyStatsHomeTeam = new SeasonStats(Guid.NewGuid(), teamId, SeasonEnum.Season2022_2023, leagueId, 2.0f, 10, 5, 3, 2, 10, 5);
        var historyStatsAwayTeam = new SeasonStats(Guid.NewGuid(), currentMatch.AwayTeamId, SeasonEnum.Season2022_2023, leagueId, 2.0f, 12, 5, 3, 2, 10, 5);
        var league = new League { Id = leagueId, LeagueStrengths = new List<LeagueStrength>(){new LeagueStrength() { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = SeasonEnum.Season2022_2023, Strength = 1.0f}} }; // No strength in DB

        // Mocks
        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRound> { new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = currentSeasonStr } });

        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(league);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MatchRound> { currentMatch });

        // Mock History Call
        mediatorMock.Setup(m => m.Send(It.Is<GetSeasonsStatsByTeamIdGrpcQuery>(q => q.teamId == teamId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SeasonStats> { historyStatsHomeTeam, historyStatsAwayTeam });

        var handler = new InitSimulationContentCommandHandler(seasonStatsService, mediatorMock.Object, loggerMock.Object);
        var command = new InitSimulationContentCommand(paramsDto);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var team = result.TeamsStrengthDictionary[teamId].OrderBy(x => x.SeasonStats.MatchesPlayed).Last();

        // Check if Merge worked: 1 goal (current) + 10 goals (history) = 11
        Assert.Equal(11, team.SeasonStats.GoalsFor);
        // Check if Matches accumulated: 1 (current) + 10 (history) = 11
        Assert.Equal(11, team.SeasonStats.MatchesPlayed);
    }
}