using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Interfaces;

public interface ISimulationStatsGrpcClient
{
    Task<(bool, Guid)> CreateSimulationTeamStatsAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<List<SimulationTeamStatsDto>> GetSimulationStatsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
