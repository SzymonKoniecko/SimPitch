using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.LeagueRounds.DTOs;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Interfaces;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;
using Xunit;

public class CreateScoreboardCommandHandlerTests
{
    private readonly Mock<IScoreboardWriteRepository> _scoreboardRepoMock = new();
    private readonly Mock<IScoreboardTeamStatsWriteRepository> _teamStatsRepoMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ILeagueRoundGrpcClient> _leagueRoundClientMock = new();
    private readonly Mock<IMatchRoundGrpcClient> _matchRoundClientMock = new();
    private readonly Mock<ILogger<CreateScoreboardCommandHandler>> _loggerMock = new();

    private readonly ScoreboardService _scoreboardService;

    public CreateScoreboardCommandHandlerTests()
    {
        _scoreboardService = new ScoreboardService(new ScoreboardTeamStatsService());
    }

    [Fact]
    public async Task Handle_ShouldCreateScoreboard_WhenValidData()
    {
        // Arrange
        var IterationResultDto = new IterationResultDto
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1.2f,
            PriorLeagueStrength = 1.1f,
            SimulationParams = new SimulationParamsDto
            {
                LeagueId = Guid.NewGuid(),
                LeagueRoundId = Guid.NewGuid(),
                SeasonYears = new List<string> { "2024/25" },
                Iterations = 1
            },
            SimulatedMatchRounds = new List<MatchRoundDto>
            {
                new MatchRoundDto
                {
                    Id = Guid.NewGuid(),
                    RoundId = Guid.NewGuid(),
                    HomeTeamId = Guid.NewGuid(),
                    AwayTeamId = Guid.NewGuid(),
                    HomeGoals = 2,
                    AwayGoals = 1,
                    IsDraw = false,
                    IsPlayed = true
                }
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IterationResultDto> { IterationResultDto });

        _leagueRoundClientMock
            .Setup(c => c.GetAllLeagueRoundsByParams(It.IsAny<LeagueRoundDtoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRoundDto> { new LeagueRoundDto { Id = Guid.NewGuid() } });

        _matchRoundClientMock
            .Setup(c => c.GetMatchRoundsByRoundIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MatchRoundDto>
            {
                new MatchRoundDto
                {
                    Id = Guid.NewGuid(),
                    RoundId = Guid.NewGuid(),
                    HomeTeamId = Guid.NewGuid(),
                    AwayTeamId = Guid.NewGuid(),
                    HomeGoals = 1,
                    AwayGoals = 1,
                    IsDraw = true,
                    IsPlayed = true
                }
            });

        var handler = new CreateScoreboardCommandHandler(
            _scoreboardRepoMock.Object,
            _mediatorMock.Object,
            _scoreboardService,
            _teamStatsRepoMock.Object,
            _loggerMock.Object,
            _leagueRoundClientMock.Object,
            _matchRoundClientMock.Object
        );

        var command = new CreateScoreboardCommand(IterationResultDto.SimulationId, Guid.Empty);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        _scoreboardRepoMock.Verify(r => r.CreateScoreboardAsync(It.IsAny<Scoreboard>(), It.IsAny<CancellationToken>()), Times.Once);
        _teamStatsRepoMock.Verify(r => r.CreateScoreboardTeamStatsBulkAsync(It.IsAny<IEnumerable<ScoreboardTeamStats>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNoLeagueRounds()
    {
        // Arrange
        var IterationResultDto = new IterationResultDto
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1.0f,
            PriorLeagueStrength = 1.0f,
            SimulationParams = new SimulationParamsDto
            {
                LeagueId = Guid.NewGuid(),
                LeagueRoundId = Guid.NewGuid(),
                SeasonYears = new List<string> { "2024/25" },
                Iterations = 1
            },
            SimulatedMatchRounds = new List<MatchRoundDto>()
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IterationResultDto> { IterationResultDto });

        _leagueRoundClientMock
            .Setup(c => c.GetAllLeagueRoundsByParams(It.IsAny<LeagueRoundDtoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRoundDto>()); // Empty

        var handler = new CreateScoreboardCommandHandler(
            _scoreboardRepoMock.Object,
            _mediatorMock.Object,
            _scoreboardService,
            _teamStatsRepoMock.Object,
            _loggerMock.Object,
            _leagueRoundClientMock.Object,
            _matchRoundClientMock.Object
        );

        var command = new CreateScoreboardCommand(IterationResultDto.SimulationId, Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
    [Fact]
    public async Task Handle_ShouldThrow_WhenNoIterationResults()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IterationResultDto>()); // brak wyników

        var handler = new CreateScoreboardCommandHandler(
            _scoreboardRepoMock.Object,
            _mediatorMock.Object,
            _scoreboardService,
            _teamStatsRepoMock.Object,
            _loggerMock.Object,
            _leagueRoundClientMock.Object,
            _matchRoundClientMock.Object
        );

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new CreateScoreboardCommand(Guid.NewGuid(), Guid.Empty), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldFilterResultsByIterationResultId()
    {
        // Arrange
        var simId = Guid.NewGuid();
        var result1 = new IterationResultDto { Id = Guid.NewGuid(), SimulationId = simId, SimulationParams = new SimulationParamsDto { LeagueId = Guid.NewGuid(), SeasonYears = new List<string>(), LeagueRoundId = Guid.NewGuid() }, SimulatedMatchRounds = new List<MatchRoundDto>(), LeagueStrength = 1, PriorLeagueStrength = 1 };
        var result2 = new IterationResultDto { Id = Guid.NewGuid(), SimulationId = simId, SimulationParams = result1.SimulationParams, SimulatedMatchRounds = new List<MatchRoundDto>(), LeagueStrength = 1, PriorLeagueStrength = 1 };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IterationResultDto> { result1, result2 });

        _leagueRoundClientMock
            .Setup(c => c.GetAllLeagueRoundsByParams(It.IsAny<LeagueRoundDtoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRoundDto> { new LeagueRoundDto { Id = Guid.NewGuid() } });

        _matchRoundClientMock
            .Setup(c => c.GetMatchRoundsByRoundIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MatchRoundDto>());

        var handler = new CreateScoreboardCommandHandler(
            _scoreboardRepoMock.Object,
            _mediatorMock.Object,
            _scoreboardService,
            _teamStatsRepoMock.Object,
            _loggerMock.Object,
            _leagueRoundClientMock.Object,
            _matchRoundClientMock.Object
        );

        var command = new CreateScoreboardCommand(simId, result2.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Single(result); // tylko jeden wynik
    }
    [Fact]
    public void CalculateSingleScoreboard_ShouldCombinePlayedAndSimulatedRounds()
    {
        // Arrange
        var service = new ScoreboardService(new ScoreboardTeamStatsService());

        var IterationResult = new IterationResult
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1.0f,
            PriorLeagueStrength = 0.9f,
            SimulatedMatchRounds = new List<MatchRound>
            {
                new MatchRound
                {
                    Id = Guid.NewGuid(),
                    HomeTeamId = Guid.NewGuid(),
                    AwayTeamId = Guid.NewGuid(),
                    HomeGoals = 3,
                    AwayGoals = 0,
                    IsDraw = false
                }
            }
        };

        var playedRounds = new List<MatchRound>
        {
            new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 1,
                AwayGoals = 1,
                IsDraw = true
            }
        };

        // Act
        var scoreboard = service.CalculateSingleScoreboard(IterationResult, playedRounds);

        // Assert
        Assert.Equal(4, scoreboard.ScoreboardTeams.Count);
    }

    [Fact]
    public void CalculateSingleScoreboard_ShouldReturnEmptyTeams_WhenNoMatches()
    {
        // Arrange
        var service = new ScoreboardService(new ScoreboardTeamStatsService());
        var sim = new IterationResult
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1,
            PriorLeagueStrength = 1,
            SimulatedMatchRounds = new List<MatchRound>()
        };

        // Act
        var scoreboard = service.CalculateSingleScoreboard(sim, new List<MatchRound>());

        // Assert
        Assert.Empty(scoreboard.ScoreboardTeams);
    }
}