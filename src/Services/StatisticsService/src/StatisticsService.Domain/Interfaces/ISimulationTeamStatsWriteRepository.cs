using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface ISimulationTeamStatsWriteRepository
{
    Task CreateSimulationTeamStatsAsync(SimulationTeamStats simulationTeamStats, CancellationToken cancellationToken);
}
