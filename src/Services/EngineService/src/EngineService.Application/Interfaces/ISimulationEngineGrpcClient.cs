using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Interfaces;

public interface ISimulationEngineGrpcClient
{
    Task<string> CreateSimulation(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken);
    Task<List<SimulationOverviewDto>> GetSimulationOverviewsAsync(CancellationToken cancellationToken);
}
