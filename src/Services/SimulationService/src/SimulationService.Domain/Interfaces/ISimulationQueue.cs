using System;
using SimulationService.Domain.Background;

namespace SimulationService.Domain.Interfaces;

public interface ISimulationQueue
{
    Task EnqueueAsync(SimulationJob job, CancellationToken cancellationToken = default);
    Task<SimulationJob?> DequeueAsync(CancellationToken cancellationToken = default);
}