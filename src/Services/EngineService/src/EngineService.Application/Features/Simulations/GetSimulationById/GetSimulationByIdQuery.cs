using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.GetSimulationById;

public record GetSimulationByIdQuery(Guid simulationId) : IRequest<SimulationDto>;