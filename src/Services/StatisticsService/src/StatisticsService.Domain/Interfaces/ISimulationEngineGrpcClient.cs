using System;
using StatisticsService.Application.DTOs.Clients;

namespace StatisticsService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<List<SimulationOverview>> GetSimulationOverviewAsync(CancellationToken cancellationToken);
}
