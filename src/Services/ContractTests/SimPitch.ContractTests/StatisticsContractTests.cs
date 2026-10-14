extern alias EngineClient;
extern alias SimulationClient;
extern alias StatisticsServer;

using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Moq;
using SimPitch.ContractTests.Infrastructure;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByLeagueIdAndSeasonYear;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using StatisticsService.Application.Features.SimulationStats.Commands;
using StatisticsService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;
using EngineClients = EngineClient::EngineService.Infrastructure.Clients;
using EngineProtos = EngineClient::SimPitchProtos.StatisticsService;
using ServerProtos = StatisticsServer::SimPitchProtos.StatisticsService;
using SimulationClients = SimulationClient::SimulationService.Infrastructure.Clients;
using SimulationProtos = SimulationClient::SimPitchProtos.StatisticsService;
using SimulationDtos = SimulationService.Application.Features;
using SimulationEnums = SimulationService.Domain.Enums;

namespace SimPitch.ContractTests;

/// <summary>
/// Contracts of StatisticsService (gRPC server) with its clients:
/// EngineService (Scoreboard + SimulationStats) and SimulationService (Scoreboard - creating scoreboards from iterations).
/// </summary>
public class StatisticsContractTests : IAsyncLifetime
{
    private GrpcContractServer _server = default!;

    public Task InitializeAsync()
    {
        _server = GrpcContractServer.Start<StatisticsService.Infrastructure.Middlewares.GrpcExceptionInterceptor>(app =>
        {
            app.MapGrpcService<StatisticsServer::StatisticsService.API.Services.ScoreboardGrpcService>();
            app.MapGrpcService<StatisticsServer::StatisticsService.API.Services.SimulationStatsGrpcService>();
        });
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _server.DisposeAsync();

    private EngineClients.ScoreboardGrpcClient EngineScoreboards =>
        new(new EngineProtos.Scoreboard.ScoreboardService.ScoreboardServiceClient(_server.Channel));

    private EngineClients.SimulationStatsGrpcClient EngineSimulationStats =>
        new(new EngineProtos.SimulationStats.SimulationStatsService.SimulationStatsServiceClient(_server.Channel));

    private SimulationClients.ScoreboardGrpcClient SimulationScoreboards =>
        new(new SimulationProtos.Scoreboard.ScoreboardService.ScoreboardServiceClient(_server.Channel));

    private static ScoreboardDto ServerScoreboard(Guid simulationId, Guid iterationResultId)
    {
        var id = Guid.NewGuid();
        // Teams deliberately not ordered by rank
        var teams = new[] { 3, 1, 2 }.Select(rank => new ScoreboardTeamStatsDto
        {
            Id = Guid.NewGuid(), ScoreboardId = id, TeamId = Guid.NewGuid(), Rank = rank, Points = 70 - rank * 10,
            MatchPlayed = 34, Wins = 20 - rank, Losses = 5 + rank, Draws = 9, GoalsFor = 60 - rank, GoalsAgainst = 30 + rank
        }).ToList();

        return new ScoreboardDto
        {
            Id = id,
            SimulationId = simulationId,
            IterationResultId = iterationResultId,
            CreatedAt = ContractData.Timestamp,
            ScoreboardTeams = teams,
            InitialScoreboardTeams = teams.Select(t => new ScoreboardTeamStatsDto { Id = Guid.NewGuid(), ScoreboardId = id, TeamId = t.TeamId, Rank = t.Rank, IsInitialStat = true }).ToList()
        };
    }

    private static void AssertScoreboardEqual(ScoreboardDto sent, EngineService.Application.DTOs.ScoreboardDto received)
    {
        Assert.Equal(sent.Id, received.Id);
        Assert.Equal(sent.SimulationId, received.SimulationId);
        Assert.Equal(sent.IterationResultId, received.IterationResultId);
        Assert.Equal(sent.CreatedAt, received.CreatedAt);

        // Engine receives the table ordered by rank
        Assert.Equal(new[] { 1, 2, 3 }, received.ScoreboardTeams.Select(t => t.Rank));
        foreach (var team in received.ScoreboardTeams)
        {
            var expected = sent.ScoreboardTeams.Single(t => t.TeamId == team.TeamId);
            Assert.Equal(
                (expected.Id, expected.ScoreboardId, expected.Points, expected.MatchPlayed, expected.Wins, expected.Losses, expected.Draws, expected.GoalsFor, expected.GoalsAgainst),
                (team.Id, team.ScoreboardId, team.Points, team.MatchPlayed, team.Wins, team.Losses, team.Draws, team.GoalsFor, team.GoalsAgainst));
        }
        Assert.Equal(sent.InitialScoreboardTeams.Count, received.InitialScoreboardTeams.Count);
    }

    // ---------- schema ----------

    [Fact]
    public void Schema_ScoreboardService_IsSameInEngineAndServerCopies()
        => ProtoSchemaContract.AssertCompatible(
            ServerProtos.Scoreboard.ScoreboardService.Descriptor,
            EngineProtos.Scoreboard.ScoreboardService.Descriptor);

    [Fact]
    public void Schema_SimulationStatsService_IsSameInEngineAndServerCopies()
        => ProtoSchemaContract.AssertCompatible(
            ServerProtos.SimulationStats.SimulationStatsService.Descriptor,
            EngineProtos.SimulationStats.SimulationStatsService.Descriptor);

    [Fact]
    public void Schema_ScoreboardService_IsSameInSimulationAndServerCopies()
        => ProtoSchemaContract.AssertCompatible(
            ServerProtos.Scoreboard.ScoreboardService.Descriptor,
            SimulationProtos.Scoreboard.ScoreboardService.Descriptor);

    // ---------- Engine -> Scoreboard ----------

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetScoreboardsByQuery_RequestAndScoreboards_CrossUnchanged(bool withTeamStats)
    {
        var simulationId = Guid.NewGuid();
        var iterationId = Guid.NewGuid();
        GetScoreboardsBySimulationIdQuery? receivedQuery = null;
        var scoreboards = new List<ScoreboardDto> { ServerScoreboard(simulationId, iterationId), ServerScoreboard(simulationId, iterationId) };
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<GetScoreboardsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<List<ScoreboardDto>>, CancellationToken>((q, _) => receivedQuery = (GetScoreboardsBySimulationIdQuery)q)
            .ReturnsAsync(scoreboards);

        var received = await EngineScoreboards.GetScoreboardsByQueryAsync(simulationId, CancellationToken.None, iterationId, withTeamStats);

        Assert.Equal(new GetScoreboardsBySimulationIdQuery(simulationId, iterationId, withTeamStats), receivedQuery);
        Assert.Equal(2, received.Count);
        for (int i = 0; i < scoreboards.Count; i++)
            AssertScoreboardEqual(scoreboards[i], received[i]);
    }

    [Fact]
    public async Task GetScoreboardsByQuery_WithoutIteration_SendsEmptyIterationId()
    {
        GetScoreboardsBySimulationIdQuery? receivedQuery = null;
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<GetScoreboardsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<List<ScoreboardDto>>, CancellationToken>((q, _) => receivedQuery = (GetScoreboardsBySimulationIdQuery)q)
            .ReturnsAsync(new List<ScoreboardDto> { ServerScoreboard(Guid.NewGuid(), Guid.Empty) });

        await EngineScoreboards.GetScoreboardsByQueryAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Equal(Guid.Empty, receivedQuery!.iterationResultId);
        Assert.False(receivedQuery.withTeamStats);
    }

    [Fact]
    public async Task GetScoreboardsByQuery_CreatesMissingScoreboard()
    {
        var simulationId = Guid.NewGuid();
        var iterationId = Guid.NewGuid();
        var created = ServerScoreboard(simulationId, iterationId);
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetScoreboardsBySimulationIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ScoreboardDto>());
        _server.Mediator.Setup(m => m.Send(It.IsAny<CreateScoreboardCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new[] { created });

        var received = await EngineScoreboards.GetScoreboardsByQueryAsync(simulationId, CancellationToken.None, iterationId, true);

        AssertScoreboardEqual(created, Assert.Single(received));
    }

    [Fact]
    public async Task CreateScoreboardByLeagueAndSeason_RequestAndScoreboard_CrossUnchanged()
    {
        var leagueId = Guid.NewGuid();
        CreateScoreboardByLeagueIdAndSeasonYearCommand? receivedCommand = null;
        var scoreboard = ServerScoreboard(Guid.Empty, Guid.Empty);
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<CreateScoreboardByLeagueIdAndSeasonYearCommand>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<ScoreboardDto>, CancellationToken>((c, _) => receivedCommand = (CreateScoreboardByLeagueIdAndSeasonYearCommand)c)
            .ReturnsAsync(scoreboard);

        var received = await EngineScoreboards.CreateScoreboardByLeagueIdAndSeasonYear(leagueId, "2025/2026", CancellationToken.None);

        Assert.Equal(new CreateScoreboardByLeagueIdAndSeasonYearCommand(leagueId, "2025/2026"), receivedCommand);
        AssertScoreboardEqual(scoreboard, received);
    }

    // ---------- Engine -> SimulationStats ----------

    [Fact]
    public async Task CreateSimulationStats_ReturnsResultOfServer()
    {
        var simulationId = Guid.NewGuid();
        _server.Mediator.Setup(m => m.Send(It.IsAny<CreateSimulationStatsCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync((false, simulationId));

        var (isCreated, id) = await EngineSimulationStats.CreateSimulationTeamStatsAsync(simulationId, CancellationToken.None);

        Assert.False(isCreated);
        Assert.Equal(simulationId, id);
    }

    [Fact]
    public async Task GetSimulationStats_IsReadByEngineUnchanged()
    {
        var simulationId = Guid.NewGuid();
        var stats = new[]
        {
            new SimulationTeamStatsDto
            {
                Id = Guid.NewGuid(), SimulationId = simulationId, TeamId = Guid.NewGuid(),
                PositionProbbility = new[] { 0.5f, 0.25f, 0.125f, 0.125f },
                AverangePoints = 61.25f, AverangeWins = 18.5f, AverangeLosses = 9.75f, AverangeDraws = 5.75f, AverangeGoalsFor = 55.5f, AverangeGoalsAgainst = 33.25f
            },
            new SimulationTeamStatsDto { Id = Guid.NewGuid(), SimulationId = simulationId, TeamId = Guid.NewGuid(), PositionProbbility = null! }
        };
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetSimulationStatsBySimulationIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(stats);

        var received = await EngineSimulationStats.GetSimulationStatsBySimulationIdAsync(simulationId, CancellationToken.None);

        Assert.Equal(2, received.Count);
        var first = received[0];
        Assert.Equal((stats[0].Id, stats[0].SimulationId, stats[0].TeamId), (first.Id, first.SimulationId, first.TeamId));
        Assert.Equal(stats[0].PositionProbbility, first.PositionProbbility);
        Assert.Equal(
            (stats[0].AverangePoints, stats[0].AverangeWins, stats[0].AverangeLosses, stats[0].AverangeDraws, stats[0].AverangeGoalsFor, stats[0].AverangeGoalsAgainst),
            (first.AverangePoints, first.AverangeWins, first.AverangeLosses, first.AverangeDraws, first.AverangeGoalsFor, first.AverangeGoalsAgainst));
        Assert.Empty(received[1].PositionProbbility); // missing probabilities arrive as an empty array
    }

    [Fact]
    public async Task MissingStats_AreSignalledAsNotFound()
    {
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetSimulationStatsBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("No stats"));

        var ex = await Assert.ThrowsAsync<RpcException>(() => EngineSimulationStats.GetSimulationStatsBySimulationIdAsync(Guid.NewGuid(), CancellationToken.None));

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }

    // ---------- Simulation -> Scoreboard (JSON payload inside the proto) ----------

    [Fact]
    public async Task CreateScoreboardFromIteration_OverviewAndIteration_AreReadByStatisticsUnchanged()
    {
        CreateScoreboardByIterationResultCommand? receivedCommand = null;
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<CreateScoreboardByIterationResultCommand>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<bool>, CancellationToken>((c, _) => receivedCommand = (CreateScoreboardByIterationResultCommand)c)
            .ReturnsAsync(true);

        var overview = new SimulationDtos.Simulations.DTOs.SimulationOverviewDto
        {
            Id = Guid.NewGuid(),
            CreatedDate = ContractData.Timestamp,
            PriorLeagueStrength = 2.6f,
            LeagueStrengths = new(),
            SimulationParams = new SimulationDtos.Simulations.DTOs.SimulationParamsDto
            {
                Title = ContractData.Title,
                SeasonYears = new() { "2025/2026", "2026/2027" },
                LeagueId = Guid.NewGuid(),
                Iterations = 100,
                LeagueRoundId = Guid.NewGuid(),
                TargetLeagueRoundId = null,
                CreateScoreboardOnCompleteIteration = true,
                ModelType = SimulationEnums.SimulationModelType.Advanced
            }
        };
        var teamId = Guid.NewGuid();
        var iteration = new SimulationDtos.IterationResults.DTOs.IterationResultDto
        {
            Id = Guid.NewGuid(),
            SimulationId = overview.Id,
            IterationIndex = 42,
            StartDate = ContractData.Timestamp,
            ExecutionTime = TimeSpan.FromMilliseconds(987),
            TeamStrengths = new()
            {
                new SimulationDtos.IterationResults.DTOs.TeamStrengthDto
                {
                    TeamId = teamId,
                    Likelihood = new() { Offensive = 1.4f, Defensive = 0.9f },
                    Posterior = new() { Offensive = 1.6f, Defensive = 1.1f },
                    ExpectedGoals = 1.6f,
                    RoundId = Guid.NewGuid(),
                    SeasonStats = new SimulationDtos.SeasonsStats.DTOs.SeasonStatsDto
                    {
                        Id = Guid.NewGuid(), TeamId = teamId, SeasonYear = SimulationEnums.SeasonEnum.Season2026_2027, LeagueId = overview.SimulationParams.LeagueId,
                        LeagueStrength = 2.7f, MatchesPlayed = 10, Wins = 6, Losses = 2, Draws = 2, GoalsFor = 18, GoalsAgainst = 9
                    }
                }
            },
            SimulatedMatchRounds = new()
            {
                new SimulationDtos.MatchRounds.DTOs.MatchRoundDto { Id = Guid.NewGuid(), RoundId = Guid.NewGuid(), HomeTeamId = teamId, AwayTeamId = Guid.NewGuid(), HomeGoals = 2, AwayGoals = 2, IsDraw = true, IsPlayed = true }
            }
        };

        var isCreated = await SimulationScoreboards.CreateScoreboardByIterationResultDataAsync(overview, iteration, CancellationToken.None);

        Assert.True(isCreated);
        Assert.NotNull(receivedCommand);

        var receivedOverview = receivedCommand!.Overview;
        Assert.Equal(overview.Id, receivedOverview.Id);
        Assert.Equal(overview.CreatedDate, receivedOverview.CreatedDate.ToUniversalTime());
        Assert.Equal(overview.SimulationParams.Title, receivedOverview.SimulationParams.Title);
        Assert.Equal(overview.SimulationParams.SeasonYears, receivedOverview.SimulationParams.SeasonYears);
        Assert.Equal(overview.SimulationParams.LeagueId, receivedOverview.SimulationParams.LeagueId);
        Assert.Equal(overview.SimulationParams.Iterations, receivedOverview.SimulationParams.Iterations);
        Assert.Equal(overview.SimulationParams.LeagueRoundId, receivedOverview.SimulationParams.LeagueRoundId);
        Assert.Null(receivedOverview.SimulationParams.TargetLeagueRoundId);
        Assert.True(receivedOverview.SimulationParams.CreateScoreboardOnCompleteIteration);

        var receivedIteration = receivedCommand.iterationResultDto;
        Assert.Equal(iteration.Id, receivedIteration.Id);
        Assert.Equal(iteration.SimulationId, receivedIteration.SimulationId);
        Assert.Equal(iteration.IterationIndex, receivedIteration.IterationIndex);
        Assert.Equal(iteration.StartDate, receivedIteration.StartDate.ToUniversalTime());
        Assert.Equal(iteration.ExecutionTime, receivedIteration.ExecutionTime);

        var match = Assert.Single(receivedIteration.SimulatedMatchRounds);
        var sentMatch = iteration.SimulatedMatchRounds[0];
        Assert.Equal(
            (sentMatch.Id, sentMatch.RoundId, sentMatch.HomeTeamId, sentMatch.AwayTeamId, sentMatch.HomeGoals, sentMatch.AwayGoals, sentMatch.IsDraw, sentMatch.IsPlayed),
            (match.Id, match.RoundId, match.HomeTeamId, match.AwayTeamId, match.HomeGoals, match.AwayGoals, match.IsDraw, match.IsPlayed));

        var team = Assert.Single(receivedIteration.TeamStrengths);
        var sentTeam = iteration.TeamStrengths[0];
        Assert.Equal(sentTeam.TeamId, team.TeamId);
        Assert.Equal(sentTeam.ExpectedGoals, team.ExpectedGoals);
        Assert.Equal(sentTeam.RoundId, team.RoundId);
        Assert.Equal((sentTeam.Likelihood.Offensive, sentTeam.Likelihood.Defensive), (team.Likelihood.Offensive, team.Likelihood.Defensive));
        Assert.Equal((sentTeam.Posterior.Offensive, sentTeam.Posterior.Defensive), (team.Posterior.Offensive, team.Posterior.Defensive));
        Assert.Equal("2026/2027", team.SeasonStats.SeasonYear);
        Assert.Equal(
            (sentTeam.SeasonStats.MatchesPlayed, sentTeam.SeasonStats.Wins, sentTeam.SeasonStats.Losses, sentTeam.SeasonStats.Draws, sentTeam.SeasonStats.GoalsFor, sentTeam.SeasonStats.GoalsAgainst),
            (team.SeasonStats.MatchesPlayed, team.SeasonStats.Wins, team.SeasonStats.Losses, team.SeasonStats.Draws, team.SeasonStats.GoalsFor, team.SeasonStats.GoalsAgainst));
    }
}
