using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationStateReadRepository
{
    Task<SimulationState> GetSimulationStateByIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
