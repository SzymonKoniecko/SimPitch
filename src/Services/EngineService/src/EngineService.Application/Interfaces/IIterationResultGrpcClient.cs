using System;
using EngineService.Application.DTOs;
namespace EngineService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<IterationResultDto> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken);
    Task<(List<IterationResultDto>, int)> GetIterationResultsBySimulationIdAsync(Guid simulationId, int offset, int limit, CancellationToken cancellationToken);
}
