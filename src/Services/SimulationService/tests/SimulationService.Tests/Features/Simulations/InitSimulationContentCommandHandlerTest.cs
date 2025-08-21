using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Services;
using SimulationService.Domain.ValueObjects;
using Xunit;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.Tests.Application.Features.Simulations
{
    public class InitSimulationContentCommandHandlerTests
    {
        private readonly SeasonStatsService _seasonStatsService;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InitSimulationContentCommandHandler _handler;

        public InitSimulationContentCommandHandlerTests()
        {
            _seasonStatsService = new SeasonStatsService(); // używamy realnego serwisu
            _mediatorMock = new Mock<IMediator>();
            _handler = new InitSimulationContentCommandHandler(_seasonStatsService, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectLeagueStrengthAndPriorLeagueStrength()
        {
            var leagueId = Guid.NewGuid();
            var seasonYear = "2023/2024";
            var roundId = Guid.NewGuid();

            var command = new InitSimulationContentCommand(new SimulationParamsDto
            {
                SeasonYear = seasonYear,
                RoundId = roundId
            });

            var leagueRounds = new List<LeagueRound>
            {
                new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = seasonYear }
            };

            var league = new League { Id = leagueId, Strength = 1.8f };

            var playedMatch = new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 2,
                AwayGoals = 1,
                IsPlayed = true
            };

            var unplayedMatch = new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 0,
                AwayGoals = 0,
                IsPlayed = false
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leagueRounds);

            _mediatorMock.Setup(m => m.Send(It.Is<GetLeagueByIdQuery>(q => q.leagueId == leagueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(league);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MatchRound> { playedMatch, unplayedMatch });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(league.Strength, result.LeagueStrength);
            Assert.True(result.PriorLeagueStrength > 0);
            Assert.Single(result.MatchRoundsToSimulate);
            Assert.Equal(unplayedMatch.Id, result.MatchRoundsToSimulate[0].Id);
            Assert.True(result.TeamsStrengthDictionary.Count >= 2);
        }

        [Fact]
        public async Task Handle_ShouldSetPriorLeagueStrengthToZero_WhenNoMatchesPlayed()
        {
            var leagueId = Guid.NewGuid();
            var seasonYear = "2023/2024";
            var roundId = Guid.NewGuid();

            var command = new InitSimulationContentCommand(new SimulationParamsDto
            {
                SeasonYear = seasonYear,
                RoundId = roundId
            });

            var leagueRounds = new List<LeagueRound>
            {
                new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = seasonYear }
            };

            var league = new League { Id = leagueId, Strength = 2.0f };

            var unplayedMatch = new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = Guid.NewGuid(),
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 0,
                AwayGoals = 0,
                IsPlayed = false
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leagueRounds);

            _mediatorMock.Setup(m => m.Send(It.Is<GetLeagueByIdQuery>(q => q.leagueId == leagueId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(league);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MatchRound> { unplayedMatch });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(league.Strength, result.LeagueStrength);
            Assert.Equal(0f, result.PriorLeagueStrength);
            Assert.Single(result.MatchRoundsToSimulate);
            Assert.Equal(unplayedMatch.Id, result.MatchRoundsToSimulate[0].Id);
        }
    }
}
