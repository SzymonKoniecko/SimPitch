using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Write;

public interface ISimulationResultWriteRepository
{
    Task CreateSimulationResultAsync(SimulationResult simulationResult, CancellationToken cancellationToken); 
}
