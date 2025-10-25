using System;
using SimulationService.Domain.Background;

namespace SimulationService.Domain.Interfaces;

public interface ISimulationQueue
{
    void Enqueue(SimulationService.Domain.Background.SimulationJob job);
    bool TryDequeue(out SimulationService.Domain.Background.SimulationJob job);
}