using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationOverviewReadRepository
{
    Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(CancellationToken cancellationToken);
}
