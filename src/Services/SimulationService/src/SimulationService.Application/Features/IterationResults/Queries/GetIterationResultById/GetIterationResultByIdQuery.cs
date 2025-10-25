using System;
using System.Net.Cache;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;

public record GetIterationResultByIdQuery(Guid iterationId) : IRequest<IterationResultDto>;