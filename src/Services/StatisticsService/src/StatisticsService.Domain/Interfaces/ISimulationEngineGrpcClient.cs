using System;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<SimulationState> GetSimulationStateByIdAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<List<SimulationOverview>> GetAllSimulationOverviewsAsync(
        int pageSize = 100,
        CancellationToken cancellationToken = default);
}

