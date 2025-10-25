using System;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Domain.Background;

public record SimulationJob(Guid SimulationId, SimulationParams Params, SimulationState State);