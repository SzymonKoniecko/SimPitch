using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Microsoft.Extensions.Logging;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;
using Xunit;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Application.Interfaces;

namespace SimulationService.Tests.Features.Simulations;
public class RunSimulationCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSimulateAllMatchesAndUpdatePosterior()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var simulationOverviewWriteMock = new Mock<ISimulationOverviewWriteRepository>();
        var simulationStateWriteMock = new Mock<ISimulationStateWriteRepository>();
        var registry = new Mock<IRedisSimulationRegistry>();
        var loggerMock = new Mock<ILogger<RunSimulationCommandHandler>>();

        var homeTeamId = Guid.NewGuid();
        var awayTeamId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        float leagueStrength = 0.75f;
        var matchRound1 = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            HomeGoals = 0,
            AwayGoals = 0,
            IsPlayed = false
        };

        var teamStrengthDict = new Dictionary<Guid, List<TeamStrength>>
        {
            [homeTeamId] = new List<TeamStrength>{ TeamStrength.Create(homeTeamId, SeasonEnum.Season2022_2023, leagueId, leagueStrength)
                .WithExpectedGoals(1.0f)},
            [awayTeamId] = new List<TeamStrength>{TeamStrength.Create(awayTeamId, SeasonEnum.Season2022_2023, leagueId, leagueStrength)
                .WithExpectedGoals(1.0f)}
        };

        var initResponse = new SimulationContent
        {
            MatchRoundsToSimulate = new List<MatchRound> { matchRound1 },
            TeamsStrengthDictionary = teamStrengthDict,
            PriorLeagueStrength = 1.5f
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<InitSimulationContentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(initResponse);

        var handler = new RunSimulationCommandHandler(mediatorMock.Object, registry.Object, loggerMock.Object, simulationStateWriteMock.Object);

        var simulationId = Guid.NewGuid();

        var command = new RunSimulationCommand(
            new SimulationOverview(),
            simulationId,
            new SimulationParamsDto
            {
                SeasonYears = new() { "2023/2024" },
                LeagueId = leagueId,
                Iterations = 1,
            },
            new SimulationState(simulationId, 0, 0f, SimulationStatus.Pending, DateTime.Now));

        // Act
        simulationId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(simulationId, Guid.Empty);
        Assert.True(initResponse.MatchRoundsToSimulate[0].IsPlayed);
        Assert.InRange(initResponse.MatchRoundsToSimulate[0].HomeGoals, 0, 10); // spodziewany zakres
        Assert.InRange(initResponse.MatchRoundsToSimulate[0].AwayGoals, 0, 10);

        var homeTeamPosterior = initResponse.TeamsStrengthDictionary[homeTeamId].First().Posterior;
        var awayTeamPosterior = initResponse.TeamsStrengthDictionary[awayTeamId].First().Posterior;

        Assert.True(homeTeamPosterior.Offensive >= 0);
        Assert.True(homeTeamPosterior.Defensive >= 0);
        Assert.True(awayTeamPosterior.Offensive >= 0);
        Assert.True(awayTeamPosterior.Defensive >= 0);
    }
}
