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
using StatisticsService.Application.DTOs.Clients;

public class CreateScoreboardCommandHandlerTests
{
    private readonly Mock<IScoreboardWriteRepository> _scoreboardRepoMock = new();
    private readonly Mock<IScoreboardTeamStatsWriteRepository> _teamStatsRepoMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ILeagueRoundGrpcClient> _leagueRoundClientMock = new();
    private readonly Mock<IMatchRoundGrpcClient> _matchRoundClientMock = new();
    private readonly Mock<ISimulationEngineGrpcClient> _simulationEngineClientMock = new();
    private readonly Mock<ILogger<CreateScoreboardCommandHandler>> _loggerMock = new();

    private readonly ScoreboardService _scoreboardService;

    public CreateScoreboardCommandHandlerTests()
    {
        _scoreboardService = new ScoreboardService(new ScoreboardTeamStatsService());
    }

    public void Setup()
    {
        // Arrange
        var IterationResultDto = new IterationResultDto
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LeagueStrength = 1.2f,
            PriorLeagueStrength = 1.1f,
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
        SimulationParams simParams = new SimulationParams
        {
            LeagueId = Guid.NewGuid(),
            LeagueRoundId = Guid.NewGuid(),
            SeasonYears = new List<string> { "2024/25" },
            Iterations = 1
        };

        SimulationOverview simulationOverview = new()
        {
            Id = IterationResultDto.SimulationId,
            Title = "",
            CreatedDate = DateTime.Now,
            SimulationParams = simParams
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<IterationResultDto> { IterationResultDto });

        _leagueRoundClientMock
            .Setup(c => c.GetAllLeagueRoundsByParams(It.IsAny<LeagueRoundDtoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LeagueRoundDto> { new LeagueRoundDto { Id = Guid.NewGuid() } });
        _simulationEngineClientMock
            .Setup(c => c.GetSimulationOverviewByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(simulationOverview);

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
    }
}