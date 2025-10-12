using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid SimulationId) : IRequest<List<IterationResultDto>>;