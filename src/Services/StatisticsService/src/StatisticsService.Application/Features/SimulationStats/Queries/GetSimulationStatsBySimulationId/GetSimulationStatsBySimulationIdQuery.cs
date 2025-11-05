using System;
using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;

public record GetSimulationStatsBySimulationIdQuery(Guid SimulationId) : IRequest<IEnumerable<SimulationTeamStatsDto>>;