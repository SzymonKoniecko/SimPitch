using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Read;

public interface ISimulationOverviewReadRepository
{
    Task<IEnumerable<SimulationOverview>> GetSimulationOverviewsAsync(CancellationToken cancellationToken);
}
