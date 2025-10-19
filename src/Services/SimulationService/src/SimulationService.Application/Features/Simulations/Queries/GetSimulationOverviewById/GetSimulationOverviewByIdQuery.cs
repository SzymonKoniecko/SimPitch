using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;

public record GetSimulationOverviewByIdQuery(Guid simulationId) : IRequest<SimulationOverviewDto>;