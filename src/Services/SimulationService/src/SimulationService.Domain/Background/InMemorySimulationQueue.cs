
using System.Collections.Concurrent;
using SimulationService.Domain.Interfaces;

namespace SimulationService.Domain.Background;

public class InMemorySimulationQueue : ISimulationQueue
{
    private readonly ConcurrentQueue<SimulationJob> _jobs = new();

    public Task EnqueueAsync(SimulationJob job, CancellationToken cancellationToken = default)
    {
        _jobs.Enqueue(job);
        return Task.CompletedTask;
    }

    public Task<SimulationJob?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        _jobs.TryDequeue(out var job);
        return Task.FromResult(job);
    }
}
