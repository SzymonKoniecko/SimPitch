using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Domain.ValueObjects;

namespace EngineService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<string> CreateSimulationAsync(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken);
    Task<(List<SimulationOverviewDto>, PagedResponseDetails)> GetPagedSimulationOverviewsAsync(PagedRequest pagedRequest, CancellationToken cancellationToken);
    Task<SimulationOverviewDto> GetSimulationOverviewBySimulationId(Guid simulationId, CancellationToken cancellationToken);
    Task<SimulationStateDto> GetSimulationStateAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<string> StopSimulationAsync(Guid simulationId, CancellationToken cancellationToken);
}
