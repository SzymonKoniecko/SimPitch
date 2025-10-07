using System;
using StatisticsService.Application.DTOs;
namespace StatisticsService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<List<IterationResultDto>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
