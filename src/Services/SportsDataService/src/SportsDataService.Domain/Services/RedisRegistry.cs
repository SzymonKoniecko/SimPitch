using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces;
using StackExchange.Redis;

namespace SportsDataService.Domain.Services;

public class RedisRegistry : IRedisRegistry
{

    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisRegistry> _logger;
    private const string KeyPrefix = "sportsdata:";

    private static string Key(string id) => $"{KeyPrefix}{id}";

    public RedisRegistry(
        IDistributedCache cache, 
        IConnectionMultiplexer redis, 
        ILogger<RedisRegistry> logger)
    {
        _cache = cache;
        _redis = redis;
        _logger = logger;
    }

    public async Task SetMatchRoundsAsync(IEnumerable<MatchRound> matchRounds, CancellationToken ct = default)
    {
        string json = JsonConvert.SerializeObject(matchRounds);
        await _cache.SetStringAsync(Key("MatchRounds"), json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        }, ct);

        _logger.LogInformation("Simulation {SimulationId} state updated", "id");
    }

    public async Task<IEnumerable<MatchRound>?> GetMatchRoundsAsync(CancellationToken ct = default)
    {
        string? json = await _cache.GetStringAsync(Key("MatchRounds"), ct);
        return json is null ? null : JsonConvert.DeserializeObject<IEnumerable<MatchRound>>(json);
    }
}
