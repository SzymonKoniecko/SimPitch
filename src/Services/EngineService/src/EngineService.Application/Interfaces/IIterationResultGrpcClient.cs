using System;
using EngineService.Application.DTOs;
namespace EngineService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<IterationResultDto> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken);
    Task<List<IterationResultDto>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
