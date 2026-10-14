using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using SimPitch.Testing;

namespace EngineService.Tests.ApiTests.Infrastructure;

[CollectionDefinition(Name)]
public class EngineApiCollection : ICollectionFixture<EngineApiFixture>
{
    public const string Name = "EngineService API";
}

/// <summary>
/// End-to-end API environment: EngineService (REST) -> real gRPC -> SimulationService -> SQL Server + Redis
/// started by Testcontainers. Only StatisticsService is replaced by a fake.
/// </summary>
public sealed class EngineApiFixture : IAsyncLifetime
{
    public SimulationStackContainers Containers { get; } = new();
    public SimulationServiceFactory Simulation { get; private set; } = default!;
    public EngineServiceFactory Engine { get; private set; } = default!;
    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await Containers.StartAsync();
        Containers.ApplyEnvironment();
        // Any valid address - requests are routed to the in-process SimulationService
        Environment.SetEnvironmentVariable("SimulationService__Address", "http://simulation-service");

        Simulation = new SimulationServiceFactory();
        Engine = new EngineServiceFactory(Simulation);
        Client = Engine.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await Engine.DisposeAsync();
        await Simulation.DisposeAsync();
        await Containers.DisposeAsync();
    }
}

public sealed class EngineServiceFactory : WebApplicationFactory<Program>
{
    private readonly SimulationServiceFactory _simulation;

    public FakeScoreboardClient Scoreboards { get; } = new();
    public FakeSimulationStatsClient SimulationStats { get; } = new();

    public EngineServiceFactory(SimulationServiceFactory simulation) => _simulation = simulation;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
        {
            // Every gRPC client created by HttpClientFactory talks to the in-process SimulationService
            services.ConfigureAll<HttpClientFactoryOptions>(options =>
                options.HttpMessageHandlerBuilderActions.Add(handlerBuilder =>
                    handlerBuilder.PrimaryHandler = _simulation.Server.CreateHandler()));

            services.RemoveAll<IScoreboardGrpcClient>();
            services.AddSingleton<IScoreboardGrpcClient>(Scoreboards);
            services.RemoveAll<ISimulationStatsGrpcClient>();
            services.AddSingleton<ISimulationStatsGrpcClient>(SimulationStats);
        });
    }
}

/// <summary>
/// StatisticsService replacement: every iteration gets a scoreboard with 4 teams ranked 1-4.
/// </summary>
public sealed class FakeScoreboardClient : IScoreboardGrpcClient
{
    public const int TeamsInScoreboard = 4;

    public Task<List<ScoreboardDto>> GetScoreboardsByQueryAsync(Guid simulationId, CancellationToken cancellationToken, Guid iterationId = default, bool? withTeamStats = null)
    {
        var scoreboardId = Guid.NewGuid();
        var scoreboard = new ScoreboardDto
        {
            Id = scoreboardId,
            SimulationId = simulationId,
            IterationResultId = iterationId,
            CreatedAt = DateTime.UtcNow,
            ScoreboardTeams = Enumerable.Range(1, TeamsInScoreboard).Select(rank => new ScoreboardTeamStatsDto
            {
                Id = Guid.NewGuid(),
                ScoreboardId = scoreboardId,
                TeamId = Guid.NewGuid(),
                Rank = rank,
                Points = (TeamsInScoreboard - rank + 1) * 10
            }).ToList()
        };
        return Task.FromResult(new List<ScoreboardDto> { scoreboard });
    }

    public Task<ScoreboardDto> CreateScoreboardByLeagueIdAndSeasonYear(Guid leagueId, string seasonYear, CancellationToken cancellationToken)
        => throw new NotSupportedException("Not used by the API tests.");
}

public sealed class FakeSimulationStatsClient : ISimulationStatsGrpcClient
{
    public Task<(bool, Guid)> CreateSimulationTeamStatsAsync(Guid simulationId, CancellationToken cancellationToken)
        => Task.FromResult((true, simulationId));

    public Task<List<SimulationTeamStatsDto>> GetSimulationStatsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
        => Task.FromResult(new List<SimulationTeamStatsDto>
        {
            new() { Id = Guid.NewGuid(), SimulationId = simulationId, TeamId = Guid.NewGuid(), PositionProbbility = new[] { 1f } }
        });
}
