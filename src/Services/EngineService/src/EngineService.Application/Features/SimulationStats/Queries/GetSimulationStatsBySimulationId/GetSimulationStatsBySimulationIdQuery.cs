using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;

public record GetSimulationStatsBySimulationIdQuery(Guid SimulationId) : IRequest<List<SimulationTeamStatsDto>>;