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
using SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;
using SimulationService.Domain.Enums;
using SimulationService.Application.Mappers;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.Consts;

namespace SimulationService.Tests.Application.Features.Simulations
{
    public class InitSimulationContentCommandHandlerTests
    {
        private readonly SeasonStatsService _seasonStatsService;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InitSimulationContentCommandHandler _handler;
        public SimulationParamsDto SimulationParams { get; set; }

        Guid leagueId = Guid.NewGuid();
        string seasonYear = "2023/2024";
        Guid roundId = Guid.NewGuid();
        Guid teamId = Guid.NewGuid();

        public InitSimulationContentCommandHandlerTests()
        {
            _seasonStatsService = new SeasonStatsService(); // używamy realnego serwisu
            _mediatorMock = new Mock<IMediator>();
            _handler = new InitSimulationContentCommandHandler(_seasonStatsService, _mediatorMock.Object);

            SimulationParams = new()
            {
                SeasonYears = new List<string>() { seasonYear },
                LeagueRoundId = roundId,
                Seed = 1000,
                GamesToReachTrust = SimulationConsts.GAMES_TO_REACH_TRUST,
                ConfidenceLevel = SimulationConsts.SIMULATION_CONFIDENCE_LEVEL,
                HomeAdvantage = SimulationConsts.HOME_ADVANTAGE,
                NoiseFactor = SimulationConsts.NOISE_FACTOR
            };
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectLeagueStrengthAndPriorLeagueStrength()
        {

            var command = new InitSimulationContentCommand(SimulationParams);

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
            var command = new InitSimulationContentCommand(SimulationParams);

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

        [Fact]
        public async Task Handle_ShouldNotModifyTeamStrength_WhenNoSeasonStatsReturned()
        {
            var command = new InitSimulationContentCommand(SimulationParams);

            var leagueRounds = new List<LeagueRound>
            {
                new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = seasonYear }
            };

            var league = new League { Id = leagueId, Strength = 1.5f };

            var match = new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = teamId,
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 1,
                AwayGoals = 0,
                IsPlayed = true
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leagueRounds);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(league);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MatchRound> { match });

            _mediatorMock.Setup(m => m.Send(It.Is<GetSeasonsStatsByTeamIdGrpcQuery>(q => q.teamId == teamId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SeasonStats>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.TeamsStrengthDictionary.ContainsKey(teamId));
            var stats = result.TeamsStrengthDictionary[teamId].First().SeasonStats;
            Assert.Equal(1, stats.GoalsFor);
        }

        [Fact]
        public async Task Handle_ShouldSkipSeasonStats_WhenSeasonYearNotInParams()
        {
            var command = new InitSimulationContentCommand(SimulationParams);

            var leagueRounds = new List<LeagueRound>
            {
                new LeagueRound { Id = Guid.NewGuid(), LeagueId = leagueId, SeasonYear = seasonYear }
            };

            var league = new League { Id = leagueId, Strength = 1.5f };

            var match = new MatchRound
            {
                Id = Guid.NewGuid(),
                HomeTeamId = teamId,
                AwayTeamId = Guid.NewGuid(),
                HomeGoals = 2,
                AwayGoals = 1,
                IsPlayed = true
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueRoundsByParamsGrpcQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leagueRounds);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLeagueByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(league);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetMatchRoundsByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MatchRound> { match });

            _mediatorMock.Setup(m => m.Send(It.Is<GetSeasonsStatsByTeamIdGrpcQuery>(q => q.teamId == teamId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SeasonStats>
                {
                    new SeasonStats(teamId, SeasonEnum.Season2022_2023, leagueId, 1.0f, 10, 5, 3, 2, 20, 15)
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            var stats = result.TeamsStrengthDictionary[teamId].First().SeasonStats;
            Assert.Equal(2, stats.GoalsFor);
        }
    }
}
