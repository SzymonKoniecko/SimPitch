using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;

namespace SimulationService.Domain.Interfaces.Write;

public interface ISimulationStateWriteRepository
{
    Task UpdateOrCreateAsync(SimulationState state, CancellationToken cancellationToken);
    Task ChangeStatusAsync(Guid simulationId, SimulationStatus newStatus, CancellationToken cancellationToken);
}
