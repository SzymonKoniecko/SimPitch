using System;
using StatisticsService.Application.DTOs;
namespace StatisticsService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<List<IterationResultDto>> GetPagedIterationResultsBySimulationIdAsync(Guid simulationId, int offset, int limit, CancellationToken cancellationToken);
    Task<List<IterationResultDto>> GetAllIterationResultsBySimulationIdAsync(
        Guid simulationId,
        int pageSize = 100,
        CancellationToken cancellationToken = default);
}
