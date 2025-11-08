using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationStateReadRepository
{
    Task<bool> IsSimulationStateCancelled(Guid simulationId, CancellationToken cancellationToken);
    Task<SimulationState> GetSimulationStateBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
