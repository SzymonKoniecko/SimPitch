using System;
using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid SimulationId) : IRequest<List<IterationResultDto>>;