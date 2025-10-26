using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;

public record GetSimulationStateBySimulationIdQuery(Guid simulationId) : IRequest<SimulationStateDto>;