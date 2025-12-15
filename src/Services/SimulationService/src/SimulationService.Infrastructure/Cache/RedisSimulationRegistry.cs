using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;
using StackExchange.Redis;

namespace SimulationService.Domain.Services;

public class RedisSimulationRegistry : IRedisSimulationRegistry
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisSimulationRegistry> _logger;
    private const string KeyPrefix = "simulation:";
    
    private const string OverviewsContext = "SetPagedSimulationOverviews"; 

    private static string Key(Guid id) => $"{KeyPrefix}{id}";

    public RedisSimulationRegistry(
        IDistributedCache cache, 
        IConnectionMultiplexer redis, 
        ILogger<RedisSimulationRegistry> logger)
    {
        _cache = cache;
        _redis = redis;
        _logger = logger;
    }

    public async Task SetStateAsync(Guid id, SimulationState state, CancellationToken ct = default)
    {
        string json = JsonConvert.SerializeObject(state);
        await _cache.SetStringAsync(Key(id), json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        }, ct);

        _logger.LogInformation("Simulation {SimulationId} state updated", id);
    }

    public async Task<SimulationState?> GetStateAsync(Guid id, CancellationToken ct = default)
    {
        string? json = await _cache.GetStringAsync(Key(id), ct);
        return json is null ? null : JsonConvert.DeserializeObject<SimulationState>(json);
    }

    public async Task SetPagedSimulationOverviews(PagedRequest pagedRequest, IEnumerable<SimulationOverview> simulationOverviews, CancellationToken ct)
    {
        string json = JsonConvert.SerializeObject(simulationOverviews);
        await _cache.SetStringAsync(GenerateKeyForPagedRequest(pagedRequest, OverviewsContext), json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        }, ct);
    }

    public async Task RemovePagedSimulationOverviews(CancellationToken ct = default)
    {
        var endPoints = _redis.GetEndPoints();
        var server = _redis.GetServer(endPoints.First());

        string pattern = $";{OverviewsContext}";

        var keys = server.Keys(pattern: pattern).ToArray();

        if (keys.Length > 0)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(keys);
            
            _logger.LogInformation("Removed {Count} cached pages for SimulationOverviews.", keys.Length);
        }
    }

    public async Task<IEnumerable<SimulationOverview>?> GetPagedSimulationOverviews(PagedRequest pagedRequest, CancellationToken ct = default)
    {
        string? json = await _cache.GetStringAsync(GenerateKeyForPagedRequest(pagedRequest, OverviewsContext), ct);
        return json is null ? null : JsonConvert.DeserializeObject<IEnumerable<SimulationOverview>>(json);
    }

    public async Task SetPagedIterationResults(PagedRequest pagedRequest, IEnumerable<IterationResult> iterationResults, CancellationToken ct)
    {
        string json = JsonConvert.SerializeObject(iterationResults);
        string context = $"SetPagedIterationResults{iterationResults.First().SimulationId}";
        
        await _cache.SetStringAsync(GenerateKeyForPagedRequest(pagedRequest, context), json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        }, ct);
    }

    public async Task<IEnumerable<IterationResult>?> GetPagedIterationResults(PagedRequest pagedRequest, Guid simulationId, CancellationToken ct = default)
    {
        string? json = await _cache.GetStringAsync(GenerateKeyForPagedRequest(pagedRequest, $"SetPagedIterationResults{simulationId}"), ct);
        return json is null ? null : JsonConvert.DeserializeObject<IEnumerable<IterationResult>>(json);
    }

    private static string GenerateKeyForPagedRequest(PagedRequest pagedRequest, string context)
    {
        // "0;20;None;Asc;None;SetPagedSimulationOverviews"
        StringBuilder stringBuilder = new();
        return stringBuilder.AppendJoin(';',
            pagedRequest.Offset,
            pagedRequest.PageSize,
            pagedRequest.SortingMethod.Condition,
            pagedRequest.SortingMethod.Order,
            pagedRequest.SortingMethod.SortingOption,
            context
        ).ToString();
    }
}