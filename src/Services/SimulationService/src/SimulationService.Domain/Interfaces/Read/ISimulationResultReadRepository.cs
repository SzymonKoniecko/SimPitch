using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationResultReadRepository
{
    Task<IEnumerable<SimulationResult>> GetSimulationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
