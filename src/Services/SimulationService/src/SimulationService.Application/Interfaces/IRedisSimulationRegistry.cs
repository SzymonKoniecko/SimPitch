using System;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Interfaces;

public interface IRedisSimulationRegistry
{
    Task SetStateAsync(Guid id, SimulationState state, CancellationToken ct = default);
    Task<SimulationState?> GetStateAsync(Guid id, CancellationToken ct = default);
}
