using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Interfaces;

public interface IRedisSimulationRegistry
{
    Task SetStateAsync(Guid id, SimulationState state, CancellationToken ct = default);
    Task<SimulationState?> GetStateAsync(Guid id, CancellationToken ct = default);
    Task SetPagedSimulationOverviews(PagedRequest pagedRequest, IEnumerable<SimulationOverview> simulationOverviews, CancellationToken ct);
    Task<IEnumerable<SimulationOverview>?> GetPagedSimulationOverviews(PagedRequest pagedRequest, CancellationToken ct = default);
    Task<IEnumerable<IterationResult>?> GetPagedIterationResults(PagedRequest pagedRequest, Guid simulationId, CancellationToken ct = default);
    Task SetPagedIterationResults(PagedRequest pagedRequest, IEnumerable<IterationResult> iterationResults, CancellationToken ct);
    Task RemovePagedSimulationOverviews(CancellationToken ct = default);
}
