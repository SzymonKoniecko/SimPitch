using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetSimulationById;

public record GetSimulationByIdQuery(Guid simulationId) : IRequest<SimulationDto>;