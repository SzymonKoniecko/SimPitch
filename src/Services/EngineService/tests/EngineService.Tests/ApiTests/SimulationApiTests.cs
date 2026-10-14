using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EngineService.Tests.ApiTests.Infrastructure;
using SimulationService.Domain.Enums;

namespace EngineService.Tests.ApiTests;

[Collection(EngineApiCollection.Name)]
public class SimulationApiTests : IAsyncLifetime
{
    private const string ProblemJson = "application/problem+json";
    private readonly EngineApiFixture _fixture;

    public SimulationApiTests(EngineApiFixture fixture) => _fixture = fixture;

    public Task InitializeAsync() => _fixture.Containers.ResetAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private HttpClient Client => _fixture.Client;

    private static object NewSimulation(string title = "Engine API test") => new
    {
        title,
        seasonYears = new[] { "2026/2027" },
        leagueId = Guid.NewGuid(),
        iterations = 5,
        seed = 42,
        gamesToReachTrust = 10,
        confidenceLevel = 0.95f,
        homeAdvantage = 1.05f,
        noiseFactor = 0.1f,
        modelType = "StandardPoisson"
    };

    private async Task<Guid> CreateSimulationAsync()
    {
        var response = await Client.PostAsJsonAsync("/api/simulation", NewSimulation());
        response.EnsureSuccessStatusCode();
        return Guid.Parse((await response.Content.ReadAsStringAsync()).Trim('"'));
    }

    private static async Task<JsonElement> ReadJsonAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    }

    [Fact]
    public async Task CreateSimulation_ReturnsNewSimulationId()
    {
        var simulationId = await CreateSimulationAsync();

        Assert.NotEqual(Guid.Empty, simulationId);
    }

    /// <summary>
    /// A just created simulation is answered with 404 until the worker starts it -
    /// the frontend relies on this to show "Simulation is being prepared".
    /// </summary>
    [Fact]
    public async Task GetSimulation_ReturnsNotFoundProblem_WhileSimulationIsBeingPrepared()
    {
        var simulationId = await CreateSimulationAsync();

        var response = await Client.GetAsync($"/api/simulation/{simulationId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetSimulation_ReturnsNotFoundProblem_ForUnknownSimulation()
    {
        var response = await Client.GetAsync($"/api/simulation/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);
        var problem = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal(404, problem.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task GetSimulation_ReturnsCompletedSimulation_WithPodiumOfEveryIteration()
    {
        var simulationId = await _fixture.Simulation.Seeder.SeedSimulationAsync(
            SimulationStatus.Completed, iterations: 3, completedIterations: 3, title: "Completed simulation");

        var simulation = await ReadJsonAsync(await Client.GetAsync(
            $"/api/simulation/{simulationId}?pageNumber=1&pageSize=10&sortingOption=CreatedDate&order=DESC"));

        Assert.Equal("Completed", simulation.GetProperty("state").GetProperty("state").GetString());
        Assert.Equal("Completed simulation", simulation.GetProperty("simulationParams").GetProperty("title").GetString());

        var previews = simulation.GetProperty("iterationPreviews");
        Assert.Equal(3, previews.GetProperty("totalCount").GetInt32());

        // Only the podium (ranks 1-3) of each iteration is previewed
        var items = previews.GetProperty("items").EnumerateArray().ToList();
        Assert.Equal(3 * 3, items.Count);
        foreach (var iteration in items.GroupBy(i => i.GetProperty("iterationId").GetString()))
        {
            Assert.Equal(new[] { 1, 2, 3 }, iteration.Select(i => i.GetProperty("rank").GetInt32()).OrderBy(r => r));
        }
    }

    [Fact]
    public async Task GetOverviews_ReturnsPagedSimulations_WithTheirState()
    {
        var start = DateTime.UtcNow.AddHours(-1);
        for (int i = 0; i < 3; i++)
        {
            await _fixture.Simulation.Seeder.SeedSimulationAsync(
                SimulationStatus.Completed, iterations: 1, completedIterations: 1, title: $"Overview {i}", createdDate: start.AddMinutes(i));
        }

        var page = await ReadJsonAsync(await Client.GetAsync("/api/simulation/overviews?pageNumber=1&pageSize=2"));

        Assert.Equal(3, page.GetProperty("totalCount").GetInt32());
        var items = page.GetProperty("items").EnumerateArray().ToList();
        Assert.Equal(new[] { "Overview 2", "Overview 1" }, items.Select(i => i.GetProperty("simulationParams").GetProperty("title").GetString()));
        Assert.All(items, i => Assert.Equal("Completed", i.GetProperty("state").GetProperty("state").GetString()));
    }

    [Fact]
    public async Task StopSimulation_CancelsSimulationWaitingInQueue()
    {
        var simulationId = await CreateSimulationAsync();

        var state = await ReadJsonAsync(await Client.DeleteAsync($"/api/simulation/stop/{simulationId}"));

        Assert.Equal("Cancelled", state.GetProperty("state").GetString());
    }

    [Fact]
    public async Task StopSimulation_ReturnsNotFoundProblem_ForUnknownSimulation()
    {
        var response = await Client.DeleteAsync($"/api/simulation/stop/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetSimulationStats_ReturnsStatsOfSimulation()
    {
        var simulationId = await _fixture.Simulation.Seeder.SeedSimulationAsync(SimulationStatus.Completed, iterations: 1, completedIterations: 1);

        var stats = await ReadJsonAsync(await Client.GetAsync($"/api/simulationStats/{simulationId}"));

        Assert.Single(stats.EnumerateArray());
        Assert.Equal(simulationId.ToString(), stats[0].GetProperty("simulationId").GetString());
    }
}
