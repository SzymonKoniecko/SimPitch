using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface ISimulationTeamStatsReadRepository
{
    Task<IEnumerable<SimulationTeamStats>> GetSimulationTeamStatsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<bool> HasExactNumberOfSimulationTeamStatsAsync(Guid simulationId, int expectedCount, CancellationToken cancellationToken);
}
