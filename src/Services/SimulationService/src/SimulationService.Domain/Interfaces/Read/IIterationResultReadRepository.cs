using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Interfaces.Read;

public interface IIterationResultReadRepository
{
    Task<IterationResult> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken);
    Task<IEnumerable<IterationResult>> GetIterationResultsBySimulationIdAsync(Guid simulationId, PagedRequest PagedRequest, CancellationToken cancellationToken);
    Task<int> GetIterationResultsCountBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
