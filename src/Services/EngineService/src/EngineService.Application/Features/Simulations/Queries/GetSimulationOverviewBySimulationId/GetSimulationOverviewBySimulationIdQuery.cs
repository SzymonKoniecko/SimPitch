using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetSimulationOverviewBySimulationId;

public record GetSimulationOverviewBySimulationIdQuery(Guid SimulationId) : IRequest<SimulationOverviewDto>;
