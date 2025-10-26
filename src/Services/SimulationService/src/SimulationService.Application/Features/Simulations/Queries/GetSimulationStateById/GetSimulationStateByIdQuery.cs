using System;
using MediatR;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationStateById;

public record GetSimulationStateByIdQuery(Guid simulationId) : IRequest<SimulationState>;