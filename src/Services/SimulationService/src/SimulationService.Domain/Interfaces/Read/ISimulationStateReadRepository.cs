using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationStateReadRepository
{
    Task<SimulationState> GetSimulationStateBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
