using System;
using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

public record GetSimulationResultsBySimulationIdQuery(Guid SimulationId) : IRequest<List<SimulationResultDto>>;