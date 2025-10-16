using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Write;

public interface ISimulationOverviewWriteRepository
{
    Task CreateSimulationOverviewAsync(SimulationOverview simulationOverview, CancellationToken cancellationToken);
}
