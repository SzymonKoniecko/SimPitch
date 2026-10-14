extern alias SimulationApi;

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Write;
using SimulationService.Domain.ValueObjects;
using SimulationService.Infrastructure.Background;
using Testcontainers.MsSql;
using Testcontainers.Redis;

// Shared by SimulationService.Tests and EngineService.Tests (linked file) - both host SimulationService in-process.
namespace SimPitch.Testing;

/// <summary>
/// Real infrastructure of SimulationService started in Docker by Testcontainers:
/// SQL Server with the SimulationDb schema taken from <c>sql/init.sql</c> and Redis.
/// The same images as in docker-compose are used.
/// </summary>
public sealed class SimulationStackContainers : IAsyncDisposable
{
    private const string SqlImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string RedisImage = "redis:6.2-alpine";

    private readonly MsSqlContainer _sql = new MsSqlBuilder().WithImage(SqlImage).Build();
    private readonly RedisContainer _redis = new RedisBuilder().WithImage(RedisImage).Build();

    public string SimulationDbConnectionString => new SqlConnectionStringBuilder(_sql.GetConnectionString())
    {
        InitialCatalog = "SimulationDb",
        MultipleActiveResultSets = true
    }.ConnectionString;

    public string RedisConnectionString => _redis.GetConnectionString();

    public async Task StartAsync()
    {
        await Task.WhenAll(_sql.StartAsync(), _redis.StartAsync());

        await using var connection = new SqlConnection(_sql.GetConnectionString());
        await connection.OpenAsync();
        foreach (var batch in SimulationDbSchema.LoadBatches())
        {
            await using var command = new SqlCommand(batch, connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    /// <summary>Removes all data so every test starts from an empty database and cache.</summary>
    public async Task ResetAsync()
    {
        await using var connection = new SqlConnection(SimulationDbConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "DELETE FROM dbo.IterationResult; DELETE FROM dbo.SimulationState; DELETE FROM dbo.SimulationOverview;",
            connection);
        await command.ExecuteNonQueryAsync();

        await _redis.ExecAsync(new[] { "redis-cli", "FLUSHALL" });
    }

    /// <summary>
    /// The services read their configuration from environment variables at startup (see ConfigHelper),
    /// so they have to be set before a WebApplicationFactory starts the host.
    /// </summary>
    public void ApplyEnvironment()
    {
        // Downstream services that are not part of these tests - nothing listens there
        const string unused = "http://localhost:1";

        Environment.SetEnvironmentVariable("ConnectionStrings__SimulationDb", SimulationDbConnectionString);
        Environment.SetEnvironmentVariable("ConnectionStrings__Redis", RedisConnectionString);
        Environment.SetEnvironmentVariable("SportsDataService__Address", unused);
        Environment.SetEnvironmentVariable("StatisticsService__Address", unused);
        Environment.SetEnvironmentVariable("SimPitchMl__Address", unused);
        Environment.SetEnvironmentVariable("GrpcLogging__Address", unused);
        Environment.SetEnvironmentVariable("GrpcLogging__SourceName", "ApiTests");
        Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", "false");
    }

    public async ValueTask DisposeAsync()
    {
        await _sql.DisposeAsync();
        await _redis.DisposeAsync();
    }
}

/// <summary>Reads the SimulationDb part of <c>sql/init.sql</c>, so tests always use the real schema.</summary>
internal static class SimulationDbSchema
{
    public static IEnumerable<string> LoadBatches()
    {
        var script = File.ReadAllText(FindInitScript());
        int start = script.IndexOf("IF DB_ID('SimulationDb')", StringComparison.Ordinal);
        int end = script.IndexOf("----StatisticsDb", start, StringComparison.Ordinal);
        if (start < 0 || end < 0)
            throw new InvalidOperationException("SimulationDb section not found in sql/init.sql");

        return Regex.Split(script[start..end], @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)
            .Where(batch => !string.IsNullOrWhiteSpace(batch));
    }

    private static string FindInitScript()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "sql", "init.sql");
            if (File.Exists(candidate))
                return candidate;
        }
        throw new FileNotFoundException("sql/init.sql not found in any parent directory of the test output.");
    }
}

/// <summary>
/// SimulationService hosted in-process. The background worker is removed - it would pick up queued
/// simulations and call SportsData/Statistics/ML services, which are not part of these tests.
/// </summary>
public sealed class SimulationServiceFactory : WebApplicationFactory<SimulationApi::Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
        {
            var worker = services.Single(d => d.ImplementationType == typeof(SimulationWorker));
            services.Remove(worker);
        });
    }

    public SimulationSeeder Seeder => new(Services);
}

/// <summary>Seeds simulations through the service's own repositories and Redis registry.</summary>
public sealed class SimulationSeeder
{
    private readonly IServiceProvider _services;

    public SimulationSeeder(IServiceProvider services) => _services = services;

    public async Task<Guid> SeedSimulationAsync(
        SimulationStatus status,
        int iterations,
        int completedIterations,
        string title = "Seeded simulation",
        DateTime? createdDate = null)
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        var simulationId = Guid.NewGuid();

        var simulationParams = new SimulationParams
        {
            Title = title,
            SeasonYears = new() { "2026/2027" },
            LeagueId = Guid.NewGuid(),
            // Same as SimulationService stores it for a request without rounds (see SimulationEngineMapper)
            LeagueRoundId = Guid.Empty,
            TargetLeagueRoundId = Guid.Empty,
            Iterations = iterations,
            Seed = 1,
            GamesToReachTrust = 10,
            ConfidenceLevel = 0.95f,
            HomeAdvantage = 1.05f,
            NoiseFactor = 0.1f,
            ModelType = nameof(SimulationModelType.StandardPoisson)
        };

        await provider.GetRequiredService<ISimulationOverviewWriteRepository>().CreateSimulationOverviewAsync(new SimulationOverview
        {
            Id = simulationId,
            CreatedDate = createdDate ?? DateTime.UtcNow,
            SimulationParams = JsonConvert.SerializeObject(simulationParams),
            LeagueStrengthsJSON = "[]",
            PriorLeagueStrength = 2.5f
        }, CancellationToken.None);

        for (int index = 1; index <= completedIterations; index++)
        {
            await AddIterationAsync(simulationId, index);
        }

        await SetStateAsync(simulationId, status, iterations, completedIterations);
        return simulationId;
    }

    public async Task AddIterationAsync(Guid simulationId, int iterationIndex)
    {
        using var scope = _services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IIterationResultWriteRepository>().CreateIterationResultAsync(new IterationResult
        {
            Id = Guid.NewGuid(),
            SimulationId = simulationId,
            IterationIndex = iterationIndex,
            StartDate = DateTime.UtcNow,
            ExecutionTime = TimeSpan.FromMilliseconds(100 + iterationIndex),
            TeamStrengths = "[]",
            SimulatedMatchRounds = "[]"
        }, CancellationToken.None);
    }

    /// <summary>Writes the state both to SQL and to the Redis registry - like the simulation worker does.</summary>
    public async Task SetStateAsync(Guid simulationId, SimulationStatus status, int iterations, int completedIterations)
    {
        using var scope = _services.CreateScope();
        var state = new SimulationState(simulationId, completedIterations, completedIterations * 100f / iterations, status, DateTime.UtcNow);

        await scope.ServiceProvider.GetRequiredService<ISimulationStateWriteRepository>().UpdateOrCreateAsync(state, CancellationToken.None);
        await scope.ServiceProvider.GetRequiredService<IRedisSimulationRegistry>().SetStateAsync(simulationId, state, CancellationToken.None);
    }
}
