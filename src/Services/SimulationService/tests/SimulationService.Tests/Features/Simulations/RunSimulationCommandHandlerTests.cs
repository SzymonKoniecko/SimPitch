using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;
using Xunit;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Enums;

namespace SimulationService.Tests.Features.Simulations;
public class RunSimulationCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSimulateAllMatchesAndUpdatePosterior()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();

        var homeTeamId = Guid.NewGuid();
        var awayTeamId = Guid.NewGuid();
        var leagueId = Guid.NewGuid();
        var matchRound1 = new MatchRound
        {
            Id = Guid.NewGuid(),
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            HomeGoals = 0,
            AwayGoals = 0,
            IsPlayed = false
        };

        var teamStrengthDict = new Dictionary<Guid, TeamStrength>
        {
            [homeTeamId] = TeamStrength.Create(homeTeamId, SeasonEnum.Season2022_2023, leagueId)
                .WithExpectedGoals(1.0f),
            [awayTeamId] = TeamStrength.Create(awayTeamId, SeasonEnum.Season2022_2023, leagueId)
                .WithExpectedGoals(1.0f)
        };

        var initResponse = new InitSimulationContentResponse
        {
            MatchRoundsToSimulate = new List<MatchRound> { matchRound1 },
            TeamsStrengthDictionary = teamStrengthDict,
            PriorLeagueStrength = 1.5f
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<InitSimulationContentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(initResponse);

        var handler = new RunSimulationCommandHandler(mediatorMock.Object);

        var command = new RunSimulationCommand(new SimulationParamsDto
        {
            SeasonYear = "2023/2024"
        });

        // Act
        var report = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(report);
        Assert.Contains(homeTeamId.ToString(), report);
        Assert.Contains(awayTeamId.ToString(), report);
        Assert.True(initResponse.MatchRoundsToSimulate[0].IsPlayed);
        Assert.InRange(initResponse.MatchRoundsToSimulate[0].HomeGoals, 0, 10); // spodziewany zakres
        Assert.InRange(initResponse.MatchRoundsToSimulate[0].AwayGoals, 0, 10);

        var homeTeamPosterior = initResponse.TeamsStrengthDictionary[homeTeamId].Posterior;
        var awayTeamPosterior = initResponse.TeamsStrengthDictionary[awayTeamId].Posterior;

        Assert.True(homeTeamPosterior.Offensive >= 0);
        Assert.True(homeTeamPosterior.Defensive >= 0);
        Assert.True(awayTeamPosterior.Offensive >= 0);
        Assert.True(awayTeamPosterior.Defensive >= 0);
    }
}
