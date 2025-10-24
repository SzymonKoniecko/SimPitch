using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface IIterationResultReadRepository
{
    Task<IterationResult> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken);
    Task<IEnumerable<IterationResult>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
