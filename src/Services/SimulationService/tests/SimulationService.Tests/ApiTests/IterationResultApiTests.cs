using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.IterationResult;
using SimulationService.Domain.Enums;
using SimulationService.Tests.ApiTests.Infrastructure;
using StackExchange.Redis;

namespace SimulationService.Tests.ApiTests;

[Collection(SimulationApiCollection.Name)]
public class IterationResultApiTests : IAsyncLifetime
{
    private readonly SimulationApiFixture _fixture;

    public IterationResultApiTests(SimulationApiFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.Containers.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    /// <summary>Reads the whole server stream (results may be split into several chunks).</summary>
    private async Task<(List<IterationResultGrpc> Items, int TotalCount)> GetPageAsync(Guid simulationId, int offset, int limit)
    {
        using var call = _fixture.Iterations.GetIterationResultsBySimulationId(new IterationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString(),
            PagedRequest = SimulationEngineApiTests.Paged(offset, limit)
        });

        var items = new List<IterationResultGrpc>();
        int totalCount = 0;
        await foreach (var chunk in call.ResponseStream.ReadAllAsync())
        {
            items.AddRange(chunk.Items);
            totalCount = chunk.Paged.TotalCount;
        }
        return (items, totalCount);
    }

    private bool HasCachedIterationPage(Guid simulationId)
    {
        var redis = _fixture.Factory.Services.GetRequiredService<IConnectionMultiplexer>();
        var server = redis.GetServer(redis.GetEndPoints().Single());
        return server.Keys(pattern: $"*SetPagedIterationResults{simulationId}*").Any();
    }

    [Fact]
    public async Task GetIterationResults_ReturnsRequestedPage_AndTotalCount()
    {
        var simulationId = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Completed, iterations: 12, completedIterations: 12);

        var firstPage = await GetPageAsync(simulationId, offset: 0, limit: 10);
        var secondPage = await GetPageAsync(simulationId, offset: 10, limit: 10);

        Assert.Equal(12, firstPage.TotalCount);
        Assert.Equal(10, firstPage.Items.Count);
        Assert.Equal(2, secondPage.Items.Count);
        Assert.All(firstPage.Items.Concat(secondPage.Items), i => Assert.Equal(simulationId.ToString(), i.SimulationId));
        Assert.Equal(12, firstPage.Items.Concat(secondPage.Items).Select(i => i.Id).Distinct().Count());
    }

    [Fact]
    public async Task GetIterationResults_ReturnsOnlyCompletedIterations()
    {
        // 5 iterations are stored, but the state says only 3 of them are completed
        var simulationId = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Running, iterations: 10, completedIterations: 5);
        await _fixture.Factory.Seeder.SetStateAsync(simulationId, SimulationStatus.Running, iterations: 10, completedIterations: 3);

        var page = await GetPageAsync(simulationId, offset: 0, limit: 10);

        Assert.Equal(3, page.TotalCount);
        Assert.Equal(3, page.Items.Count);
        Assert.All(page.Items, i => Assert.True(i.IterationIndex <= 3));
    }

    /// <summary>
    /// Regression: a page of a running simulation must not be cached - otherwise the same, partially
    /// filled page was served for minutes after the simulation completed.
    /// </summary>
    [Fact]
    public async Task GetIterationResults_OfRunningSimulation_AreNotCached_AndShowNewIterations()
    {
        var simulationId = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Running, iterations: 10, completedIterations: 3);

        var before = await GetPageAsync(simulationId, offset: 0, limit: 10);

        await _fixture.Factory.Seeder.AddIterationAsync(simulationId, 4);
        await _fixture.Factory.Seeder.AddIterationAsync(simulationId, 5);
        await _fixture.Factory.Seeder.SetStateAsync(simulationId, SimulationStatus.Completed, iterations: 5, completedIterations: 5);

        var after = await GetPageAsync(simulationId, offset: 0, limit: 10);

        Assert.Equal(3, before.Items.Count);
        Assert.Equal(5, after.Items.Count);
        Assert.Equal(5, after.TotalCount);
    }

    [Fact]
    public async Task GetIterationResults_OfFinishedSimulation_AreCached()
    {
        var running = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Running, iterations: 10, completedIterations: 2);
        var completed = await _fixture.Factory.Seeder.SeedSimulationAsync(SimulationStatus.Completed, iterations: 2, completedIterations: 2);

        await GetPageAsync(running, offset: 0, limit: 10);
        await GetPageAsync(completed, offset: 0, limit: 10);

        Assert.False(HasCachedIterationPage(running));
        Assert.True(HasCachedIterationPage(completed));
    }

    [Fact]
    public async Task GetIterationResultById_ReturnsNotFound_ForUnknownIteration()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(() => _fixture.Iterations.GetIterationResultByIdAsync(
            new IterationResultByIdRequest { Id = Guid.NewGuid().ToString() }).ResponseAsync);

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }
}
