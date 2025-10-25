
using System.Collections.Concurrent;
using SimulationService.Domain.Interfaces;

namespace SimulationService.Domain.Background;

public class InMemorySimulationQueue : ISimulationQueue
{
    private readonly ConcurrentQueue<SimulationJob> _jobs = new();

    public void Enqueue(SimulationJob job) => _jobs.Enqueue(job);

    public bool TryDequeue(out SimulationJob job) => _jobs.TryDequeue(out job);
}