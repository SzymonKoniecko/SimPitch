extern alias EngineClient;
extern alias SimulationClient;
extern alias SimulationServer;

using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Moq;
using SimPitch.ContractTests.Infrastructure;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Application.Features.Simulations.Commands.SetSimulation;
using SimulationService.Application.Features.Simulations.Commands.StopSimulation;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;
using SimulationService.Domain.Enums;
using SimulationService.Domain.ValueObjects;
using EngineClients = EngineClient::EngineService.Infrastructure.Clients;
using EngineDtos = EngineService.Application.DTOs;
using EnginePaging = EngineService.Application.Common.Pagination;
using ClientProtos = EngineClient::SimPitchProtos.SimulationService;
using ServerProtos = SimulationServer::SimPitchProtos.SimulationService;

namespace SimPitch.ContractTests;

/// <summary>
/// Contract between EngineService (gRPC client) and SimulationService (gRPC server):
/// SimulationEngine + IterationResult services.
/// </summary>
public class EngineToSimulationContractTests : IAsyncLifetime
{
    private GrpcContractServer _server = default!;
    private EngineClients.SimulationEngineGrpcClient _engineClient = default!;
    private EngineClients.IterationResultGrpcClient _iterationClient = default!;

    public Task InitializeAsync()
    {
        _server = GrpcContractServer.Start<SimulationClient::SimulationService.Infrastructure.Middlewares.GrpcExceptionInterceptor>(app =>
        {
            app.MapGrpcService<SimulationServer::SimulationService.API.Services.SimulationEngineGrpcService>();
            app.MapGrpcService<SimulationServer::SimulationService.API.Services.IterationResultGrpcService>();
        });
        _engineClient = new(new ClientProtos.SimulationEngine.SimulationEngineService.SimulationEngineServiceClient(_server.Channel));
        _iterationClient = new(new ClientProtos.IterationResult.IterationResultService.IterationResultServiceClient(_server.Channel));
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _server.DisposeAsync();

    private static SimulationParamsDto ServerParams(bool createScoreboards, Guid? leagueRoundId, Guid? targetLeagueRoundId) => new()
    {
        Title = ContractData.Title,
        SeasonYears = new() { "2025/2026", "2026/2027" },
        LeagueId = Guid.NewGuid(),
        Iterations = 250,
        LeagueRoundId = leagueRoundId,
        TargetLeagueRoundId = targetLeagueRoundId,
        CreateScoreboardOnCompleteIteration = createScoreboards,
        Seed = 291363,
        GamesToReachTrust = 15,
        ConfidenceLevel = 0.95f,
        HomeAdvantage = 1.05f,
        NoiseFactor = 0.12f,
        ModelType = SimulationModelType.DixonColes
    };

    private static SimulationOverviewDto ServerOverview(SimulationParamsDto simulationParams) => new()
    {
        Id = Guid.NewGuid(),
        CreatedDate = ContractData.Timestamp,
        SimulationParams = simulationParams,
        LeagueStrengths = new() { new LeagueStrengthDto { Id = Guid.NewGuid(), LeagueId = simulationParams.LeagueId, SeasonYear = "2026/2027", Strength = 2.75f } },
        PriorLeagueStrength = 2.6f
    };

    private static void AssertParamsEqual(SimulationParamsDto sent, EngineDtos.SimulationParamsDto received)
    {
        Assert.Equal(sent.Title, received.Title);
        Assert.Equal(sent.SeasonYears, received.SeasonYears);
        Assert.Equal(sent.LeagueId, received.LeagueId);
        Assert.Equal(sent.Iterations, received.Iterations);
        Assert.Equal(sent.LeagueRoundId ?? Guid.Empty, received.LeagueRoundId ?? Guid.Empty);
        Assert.Equal(sent.TargetLeagueRoundId ?? Guid.Empty, received.TargetLeagueRoundId ?? Guid.Empty);
        Assert.Equal(sent.CreateScoreboardOnCompleteIteration, received.CreateScoreboardOnCompleteIteration);
        Assert.Equal(sent.Seed, received.Seed);
        Assert.Equal(sent.GamesToReachTrust, received.GamesToReachTrust);
        Assert.Equal(sent.ConfidenceLevel, received.ConfidenceLevel);
        Assert.Equal(sent.HomeAdvantage, received.HomeAdvantage);
        Assert.Equal(sent.NoiseFactor, received.NoiseFactor);
        Assert.Equal(sent.ModelType.ToString(), received.ModelType);
    }

    // ---------- schema ----------

    [Fact]
    public void Schema_SimulationEngineService_IsSameInClientAndServerCopies()
        => ProtoSchemaContract.AssertCompatible(
            ServerProtos.SimulationEngine.SimulationEngineService.Descriptor,
            ClientProtos.SimulationEngine.SimulationEngineService.Descriptor);

    [Fact]
    public void Schema_IterationResultService_IsSameInClientAndServerCopies()
        => ProtoSchemaContract.AssertCompatible(
            ServerProtos.IterationResult.IterationResultService.Descriptor,
            ClientProtos.IterationResult.IterationResultService.Descriptor);

    // ---------- SimulationEngine ----------

    [Fact]
    public async Task GetSimulationState_IsReadByEngineUnchanged()
    {
        var state = new SimulationStateDto
        {
            Id = Guid.NewGuid(),
            SimulationId = Guid.NewGuid(),
            LastCompletedIteration = 17,
            ProgressPercent = 42.5f,
            State = "Running",
            UpdatedAt = ContractData.Timestamp
        };
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetSimulationStateBySimulationIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(state);

        var received = await _engineClient.GetSimulationStateAsync(state.SimulationId, CancellationToken.None);

        Assert.Equal(state.Id, received.Id);
        Assert.Equal(state.SimulationId, received.SimulationId);
        Assert.Equal(state.LastCompletedIteration, received.LastCompletedIteration);
        Assert.Equal(state.ProgressPercent, received.ProgressPercent);
        Assert.Equal(state.State, received.State);
        Assert.Equal(state.UpdatedAt, received.UpdatedAt);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task GetSimulationOverview_IsReadByEngineUnchanged(bool createScoreboards, bool withRounds)
    {
        var overview = ServerOverview(ServerParams(
            createScoreboards,
            leagueRoundId: withRounds ? Guid.NewGuid() : null,
            targetLeagueRoundId: withRounds ? Guid.NewGuid() : null));
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetSimulationOverviewByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(overview);

        var received = await _engineClient.GetSimulationOverviewBySimulationId(overview.Id, CancellationToken.None);

        Assert.Equal(overview.Id, received.Id);
        Assert.Equal(overview.CreatedDate, received.CreatedDate);
        Assert.Equal(overview.PriorLeagueStrength, received.PriorLeagueStrength);
        AssertParamsEqual(overview.SimulationParams, received.SimulationParams);
        var strength = Assert.Single(received.LeagueStrengths);
        Assert.Equal(overview.LeagueStrengths[0].Id, strength.Id);
        Assert.Equal(overview.LeagueStrengths[0].SeasonYear, strength.SeasonYear);
        Assert.Equal(overview.LeagueStrengths[0].Strength, strength.Strength);
    }

    [Fact]
    public async Task GetPagedOverviews_PassesPagingBothWays()
    {
        PagedRequestDto? receivedRequest = null;
        var overviews = new[] { ServerOverview(ServerParams(true, null, null)), ServerOverview(ServerParams(false, null, null)) };
        var details = new PagedResponseDetails { TotalCount = 37, PageNumber = 3, PageSize = 2, SortingOption = "Title", Order = "ASC" };
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<GetAllSimulationOverviewsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<(IEnumerable<SimulationOverviewDto>, PagedResponseDetails)>, CancellationToken>((q, _) => receivedRequest = ((GetAllSimulationOverviewsQuery)q).PagedRequest)
            .ReturnsAsync((overviews.AsEnumerable(), details));

        var (items, paged) = await _engineClient.GetPagedSimulationOverviewsAsync(
            new EnginePaging.PagedRequest(offset: 4, pageSize: 2, "Title", "ślązak", "ASC"), CancellationToken.None);

        Assert.NotNull(receivedRequest);
        Assert.Equal(4, receivedRequest!.Offset);
        Assert.Equal(2, receivedRequest.PageSize);
        Assert.Equal("Title", receivedRequest.SortingMethod.SortingOption);
        Assert.Equal("ślązak", receivedRequest.SortingMethod.Condition);
        Assert.Equal("ASC", receivedRequest.SortingMethod.Order);

        Assert.Equal(overviews.Select(o => o.Id), items.Select(o => o.Id));
        Assert.Equal(37, paged.TotalCount);
        Assert.Equal("Title", paged.SortingOption);
        Assert.Equal("ASC", paged.Order);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task CreateSimulation_ParamsSentByEngine_AreReadByServerUnchanged(bool createScoreboards, bool withRounds)
    {
        SimulationParamsDto? receivedParams = null;
        var simulationId = Guid.NewGuid();
        _server.Mediator
            .Setup(m => m.Send(It.IsAny<SetSimulationCommand>(), It.IsAny<CancellationToken>()))
            .Callback<MediatR.IRequest<Guid>, CancellationToken>((c, _) => receivedParams = ((SetSimulationCommand)c).SimulationParamsDto)
            .ReturnsAsync(simulationId);

        var sent = new EngineDtos.SimulationParamsDto
        {
            Title = ContractData.Title,
            SeasonYears = new() { "2026/2027" },
            LeagueId = Guid.NewGuid(),
            Iterations = 1000,
            LeagueRoundId = withRounds ? Guid.NewGuid() : null,
            TargetLeagueRoundId = withRounds ? Guid.NewGuid() : null,
            CreateScoreboardOnCompleteIteration = createScoreboards,
            Seed = 7,
            GamesToReachTrust = 12,
            ConfidenceLevel = 0.9f,
            HomeAdvantage = 1.1f,
            NoiseFactor = 0.2f,
            ModelType = nameof(SimulationModelType.BivariatePoisson)
        };

        var response = await _engineClient.CreateSimulationAsync(sent, CancellationToken.None);

        Assert.Equal(simulationId.ToString(), response);
        Assert.NotNull(receivedParams);
        Assert.Equal(sent.Title, receivedParams!.Title);
        Assert.Equal(sent.SeasonYears, receivedParams.SeasonYears);
        Assert.Equal(sent.LeagueId, receivedParams.LeagueId);
        Assert.Equal(sent.Iterations, receivedParams.Iterations);
        Assert.Equal(sent.LeagueRoundId ?? Guid.Empty, receivedParams.LeagueRoundId ?? Guid.Empty);
        Assert.Equal(sent.TargetLeagueRoundId ?? Guid.Empty, receivedParams.TargetLeagueRoundId ?? Guid.Empty);
        Assert.Equal(sent.CreateScoreboardOnCompleteIteration, receivedParams.CreateScoreboardOnCompleteIteration);
        Assert.Equal(sent.Seed, receivedParams.Seed);
        Assert.Equal(sent.GamesToReachTrust, receivedParams.GamesToReachTrust);
        Assert.Equal(sent.ConfidenceLevel, receivedParams.ConfidenceLevel);
        Assert.Equal(sent.HomeAdvantage, receivedParams.HomeAdvantage);
        Assert.Equal(sent.NoiseFactor, receivedParams.NoiseFactor);
        Assert.Equal(SimulationModelType.BivariatePoisson, receivedParams.ModelType);
    }

    [Fact]
    public async Task StopSimulation_ReturnsServerStatus()
    {
        _server.Mediator.Setup(m => m.Send(It.IsAny<StopSimulationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("Cancelled");

        Assert.Equal("Cancelled", await _engineClient.StopSimulationAsync(Guid.NewGuid(), CancellationToken.None));
    }

    /// <summary>EngineService turns NotFound into HTTP 404 ("simulation is being prepared" in the UI).</summary>
    [Fact]
    public async Task MissingSimulation_IsSignalledAsNotFound()
    {
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetSimulationStateBySimulationIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("No simulation state found"));

        var ex = await Assert.ThrowsAsync<RpcException>(() => _engineClient.GetSimulationStateAsync(Guid.NewGuid(), CancellationToken.None));

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }

    // ---------- IterationResult ----------

    private static IterationResultDto ServerIteration(Guid simulationId, int index) => new()
    {
        Id = Guid.NewGuid(),
        SimulationId = simulationId,
        IterationIndex = index,
        StartDate = ContractData.Timestamp.AddSeconds(index),
        ExecutionTime = TimeSpan.FromMilliseconds(1234.5 + index),
        TeamStrengths = new()
        {
            new TeamStrengthDto
            {
                TeamId = Guid.NewGuid(),
                Likelihood = new StrengthItemDto { Offensive = 1.4f, Defensive = 0.9f },
                Posterior = new StrengthItemDto { Offensive = 1.6f, Defensive = 1.1f },
                ExpectedGoals = 1.6f,
                LastUpdate = ContractData.Timestamp,
                RoundId = Guid.NewGuid(),
                SeasonStats = new SeasonStatsDto
                {
                    Id = Guid.NewGuid(), TeamId = Guid.NewGuid(), SeasonYear = SeasonEnum.Season2026_2027, LeagueId = Guid.NewGuid(),
                    LeagueStrength = 2.7f, MatchesPlayed = 10, Wins = 6, Losses = 2, Draws = 2, GoalsFor = 18, GoalsAgainst = 9
                }
            }
        },
        SimulatedMatchRounds = new()
        {
            new MatchRoundDto { Id = Guid.NewGuid(), RoundId = Guid.NewGuid(), HomeTeamId = Guid.NewGuid(), AwayTeamId = Guid.NewGuid(), HomeGoals = 3, AwayGoals = 1, IsDraw = false, IsPlayed = true },
            new MatchRoundDto { Id = Guid.NewGuid(), RoundId = Guid.NewGuid(), HomeTeamId = Guid.NewGuid(), AwayTeamId = Guid.NewGuid(), HomeGoals = null, AwayGoals = null, IsDraw = null, IsPlayed = false }
        }
    };

    private static void AssertIterationEqual(IterationResultDto sent, EngineDtos.IterationResultDto received)
    {
        Assert.Equal(sent.Id, received.Id);
        Assert.Equal(sent.SimulationId, received.SimulationId);
        Assert.Equal(sent.IterationIndex, received.IterationIndex);
        Assert.Equal(sent.StartDate, received.StartDate.ToUniversalTime());
        Assert.Equal(sent.ExecutionTime, received.ExecutionTime);

        var sentTeam = sent.TeamStrengths.Single();
        var team = Assert.Single(received.TeamStrengths);
        Assert.Equal(sentTeam.TeamId, team.TeamId);
        Assert.Equal(sentTeam.Likelihood.Offensive, team.Likelihood.Offensive);
        Assert.Equal(sentTeam.Posterior.Defensive, team.Posterior.Defensive);
        Assert.Equal(sentTeam.ExpectedGoals, team.ExpectedGoals);
        Assert.Equal(sentTeam.RoundId, team.RoundId);
        Assert.Equal(sentTeam.SeasonStats.GoalsFor, team.SeasonStats.GoalsFor);
        Assert.Equal(sentTeam.SeasonStats.MatchesPlayed, team.SeasonStats.MatchesPlayed);
        Assert.Equal("2026/2027", team.SeasonStats.SeasonYear);

        Assert.Equal(
            sent.SimulatedMatchRounds.Select(m => (m.Id, m.HomeTeamId, m.AwayTeamId, m.HomeGoals, m.AwayGoals, m.IsDraw, m.IsPlayed)),
            received.SimulatedMatchRounds.Select(m => (m.Id, m.HomeTeamId, m.AwayTeamId, m.HomeGoals, m.AwayGoals, m.IsDraw, m.IsPlayed)));
    }

    [Fact]
    public async Task GetIterationResultById_IsReadByEngineUnchanged()
    {
        var iteration = ServerIteration(Guid.NewGuid(), 1);
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetIterationResultByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(iteration);

        var received = await _iterationClient.GetIterationResultByIdAsync(iteration.Id, CancellationToken.None);

        AssertIterationEqual(iteration, received);
    }

    [Fact]
    public async Task GetIterationResults_StreamsAllItems_AndPaging()
    {
        var simulationId = Guid.NewGuid();
        var iterations = Enumerable.Range(1, 25).Select(i => ServerIteration(simulationId, i)).ToList();
        var details = new PagedResponseDetails { TotalCount = 500, PageNumber = 1, PageSize = 25, SortingOption = "CreatedDate", Order = "DESC" };
        _server.Mediator.Setup(m => m.Send(It.IsAny<GetIterationResultsBySimulationIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((iterations, details));

        var (received, paged) = await _iterationClient.GetIterationResultsBySimulationIdAsync(
            simulationId, new EnginePaging.PagedRequest(0, 25, "CreatedDate", "", "DESC"), CancellationToken.None);

        Assert.Equal(25, received.Count);
        for (int i = 0; i < iterations.Count; i++)
            AssertIterationEqual(iterations[i], received[i]);
        Assert.Equal(500, paged.TotalCount);
    }
}
