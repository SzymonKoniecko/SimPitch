using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Services;

public class RedisSimulationRegistry : IRedisSimulationRegistry
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisSimulationRegistry> _logger;
    private const string KeyPrefix = "simulation:";
    private static string Key(Guid id) => $"{KeyPrefix}{id}";

    public RedisSimulationRegistry(IDistributedCache cache, ILogger<RedisSimulationRegistry> logger)
    {
        _cache = cache;
        _logger = logger;
    }


    public async Task SetStateAsync(Guid id, SimulationState state, CancellationToken ct = default)
    {
        string json = JsonConvert.SerializeObject(state);
        await _cache.SetStringAsync(Key(id), json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        }, ct);

        _logger.LogInformation("Simulation {SimulationId} state updated: {@State}", id, json);
    }

    public async Task<SimulationState?> GetStateAsync(Guid id, CancellationToken ct = default)
    {
        string? json = await _cache.GetStringAsync(Key(id), ct);
        return json is null ? null : JsonConvert.DeserializeObject<SimulationState>(json);
    }
}