using System;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SimulationService.Domain.Interfaces;
using StackExchange.Redis;

namespace SimulationService.Domain.Background;

public class RedisSimulationQueue : ISimulationQueue
{
    private readonly IDatabase _db;
    private const string QueueKey = "simulation:queue";

    public RedisSimulationQueue(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task EnqueueAsync(SimulationJob job, CancellationToken cancellationToken = default)
    {
        string payload = JsonConvert.SerializeObject(job);
        await _db.ListRightPushAsync(QueueKey, payload);
    }

    public async Task<SimulationJob?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var result = await _db.ListLeftPopAsync(QueueKey);
        if (result.IsNullOrEmpty) return null;
        return JsonConvert.DeserializeObject<SimulationJob>(result.ToString()!);
    }
}