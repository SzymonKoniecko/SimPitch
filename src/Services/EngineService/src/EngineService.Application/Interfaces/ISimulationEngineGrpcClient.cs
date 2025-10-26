using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<string> CreateSimulationAsync(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken);
    Task<List<SimulationOverviewDto>> GetSimulationOverviewsAsync(CancellationToken cancellationToken);
    Task<SimulationStateDto> GetSimulationStateAsync(Guid simulationId, CancellationToken cancellationToken);
    Task<string> StopSimulationAsync(Guid simulationId, CancellationToken cancellationToken);
}
