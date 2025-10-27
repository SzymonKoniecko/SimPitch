using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid SimulationId, int offset, int limit) : IRequest<(List<IterationResultDto>, int)>;