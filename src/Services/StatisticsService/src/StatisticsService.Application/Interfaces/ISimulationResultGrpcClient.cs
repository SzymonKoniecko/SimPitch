using System;
using StatisticsService.Application.DTOs;
namespace StatisticsService.Application.Interfaces;

public interface ISimulationResultGrpcClient
{
    Task<List<SimulationResultDto>> GetSimulationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
