using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<string> CreateSimulationAsync(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken);
    Task<(List<SimulationOverviewDto>, int)> GetPagedSimulationOverviewsAsync(int offset, int limit, CancellationToken cancellationToken);
    Task<SimulationOverviewDto> GetSimulationOverviewBySimulationId(Guid simulationId, CancellationToken cancellationToken);
    Task<SimulationStateDto> GetSimulationStateAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<string> StopSimulationAsync(Guid simulationId, CancellationToken cancellationToken);
}
