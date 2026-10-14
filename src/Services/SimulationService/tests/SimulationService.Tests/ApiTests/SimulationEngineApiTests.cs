using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Enums;
using SimulationService.Tests.ApiTests.Infrastructure;
using StackExchange.Redis;

namespace SimulationService.Tests.ApiTests;

[Collection(SimulationApiCollection.Name)]
public class SimulationEngineApiTests : IAsyncLifetime
{
    private readonly SimulationApiFixture _fixture;

    public SimulationEngineApiTests(SimulationApiFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.Containers.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private static RunSimulationEngineRequest NewSimulationRequest(string title = "API test simulation") => new()
    {
        SimulationParams = new SimulationParamsGrpc
        {
            Title = title,
            SeasonYears = { "2026/2027" },
            LeagueId = Guid.NewGuid().ToString(),
            Iterations = 5,
            Seed = 42,
            GamesToReachTrust = 10,
            ConfidenceLevel = 0.95f,
            HomeAdvantage = 1.05f,
            NoiseFactor = 0.1f,
            Model = nameof(SimulationModelType.StandardPoisson)
        }
    };

    private async Task<SimulationStatus?> GetRegistryStateAsync(Guid simulationId)
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var state = await scope.ServiceProvider.GetRequiredService<IRedisSimulationRegistry>().GetStateAsync(simulationId);
        return state?.State;
    }

    [Fact]
    public async Task GetSimulationStateById_ReturnsNotFound_ForUnknownSimulation()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(() => _fixture.Engine.GetSimulationStateByIdAsync(
            new GetSimulationStateByIdRequest { SimulationId = Guid.NewGuid().ToString() }).ResponseAsync);

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }

    [Fact]
    public async Task GetSimulationStateById_ReturnsStoredState()
    {
        var simulationId = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Completed, iterations: 4, completedIterations: 4);

        var response = await _fixture.Engine.GetSimulationStateByIdAsync(
            new GetSimulationStateByIdRequest { SimulationId = simulationId.ToString() });

        Assert.Equal(simulationId.ToString(), response.SimulationState.SimulationId);
        Assert.Equal("Completed", response.SimulationState.State);
        Assert.Equal(4, response.SimulationState.LastCompletedIteration);
        Assert.Equal(100f, response.SimulationState.ProgressPercent, precision: 2);
    }

    [Fact]
    public async Task RunSimulation_QueuesJob_AsPending()
    {
        var response = await _fixture.Engine.RunSimulationAsync(NewSimulationRequest());

        var simulationId = Guid.Parse(response.SimulationId);
        Assert.Equal(SimulationStatus.Pending, await GetRegistryStateAsync(simulationId));

        var redis = _fixture.Factory.Services.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
        Assert.Equal(1, await redis.ListLengthAsync("simulation:queue"));
    }

    /// <summary>
    /// Until the background worker picks the job up, the simulation is not stored in SQL yet - the API answers
    /// NotFound and the frontend shows "Simulation is being prepared" and polls again.
    /// </summary>
    [Fact]
    public async Task RunSimulation_StateIsNotAvailableUntilWorkerStartsTheSimulation()
    {
        var response = await _fixture.Engine.RunSimulationAsync(NewSimulationRequest());

        var ex = await Assert.ThrowsAsync<RpcException>(() => _fixture.Engine.GetSimulationStateByIdAsync(
            new GetSimulationStateByIdRequest { SimulationId = response.SimulationId }).ResponseAsync);

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }

    [Fact]
    public async Task StopSimulationById_CancelsPendingSimulation()
    {
        var run = await _fixture.Engine.RunSimulationAsync(NewSimulationRequest());

        var stop = await _fixture.Engine.StopSimulationByIdAsync(new StopSimulationRequest { SimulationId = run.SimulationId });

        Assert.Equal(nameof(SimulationStatus.Cancelled), stop.Status);
        Assert.Equal(SimulationStatus.Cancelled, await GetRegistryStateAsync(Guid.Parse(run.SimulationId)));

        var state = await _fixture.Engine.GetSimulationStateByIdAsync(new GetSimulationStateByIdRequest { SimulationId = run.SimulationId });
        Assert.Equal("Cancelled", state.SimulationState.State);
    }

    [Fact]
    public async Task StopSimulationById_KeepsTerminalState()
    {
        var simulationId = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Completed, iterations: 2, completedIterations: 2);

        var stop = await _fixture.Engine.StopSimulationByIdAsync(new StopSimulationRequest { SimulationId = simulationId.ToString() });

        Assert.Equal(nameof(SimulationStatus.Completed), stop.Status);
        Assert.Equal(SimulationStatus.Completed, await GetRegistryStateAsync(simulationId));
    }

    [Fact]
    public async Task StopSimulationById_ReturnsNotFound_ForUnknownSimulation()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(() => _fixture.Engine.StopSimulationByIdAsync(
            new StopSimulationRequest { SimulationId = Guid.NewGuid().ToString() }).ResponseAsync);

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }

    [Fact]
    public async Task GetAllSimulationOverviews_ReturnsPagesNewestFirst()
    {
        var start = DateTime.UtcNow.AddHours(-1);
        for (int i = 0; i < 7; i++)
        {
            await _fixture.Factory.Seeder.SeedSimulationAsync(
                SimulationStatus.Completed, iterations: 1, completedIterations: 1, title: $"Simulation {i}", createdDate: start.AddMinutes(i));
        }

        var firstPage = await _fixture.Engine.GetAllSimulationOverviewsAsync(Paged(offset: 0, limit: 5));
        var secondPage = await _fixture.Engine.GetAllSimulationOverviewsAsync(Paged(offset: 5, limit: 5));

        Assert.Equal(7, firstPage.Paged.TotalCount);
        Assert.Equal(
            new[] { "Simulation 6", "Simulation 5", "Simulation 4", "Simulation 3", "Simulation 2" },
            firstPage.Items.Select(o => o.SimulationParams.Title));
        Assert.Equal(new[] { "Simulation 1", "Simulation 0" }, secondPage.Items.Select(o => o.SimulationParams.Title));
    }

    internal static PagedRequestGrpc Paged(int offset, int limit) => new()
    {
        Offset = offset,
        Limit = limit,
        SortingMethod = new SortingMethodGrpc { SortingOption = "CreatedDate", Order = "DESC", Condition = "" }
    };
}
