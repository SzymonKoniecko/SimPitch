using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Domain.Interfaces.Write;

public interface IIterationResultWriteRepository
{
    Task CreateIterationResultAsync(IterationResult IterationResult, CancellationToken cancellationToken); 
}
