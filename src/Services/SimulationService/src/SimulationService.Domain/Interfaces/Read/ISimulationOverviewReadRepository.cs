using System;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationOverviewReadRepository
{
    Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(PagedRequest pagedRequest, CancellationToken cancellationToken);
    Task<int> GetSimulationOverviewCountAsync(CancellationToken cancellationToken);
}
