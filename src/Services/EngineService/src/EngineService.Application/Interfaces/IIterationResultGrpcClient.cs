using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Domain.ValueObjects;
namespace EngineService.Application.Interfaces;

public interface IIterationResultGrpcClient
{
    Task<IterationResultDto> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken);
    Task<(List<IterationResultDto>, PagedResponseDetails)> GetIterationResultsBySimulationIdAsync(Guid simulationId, PagedRequest pagedRequest, CancellationToken cancellationToken);
}
