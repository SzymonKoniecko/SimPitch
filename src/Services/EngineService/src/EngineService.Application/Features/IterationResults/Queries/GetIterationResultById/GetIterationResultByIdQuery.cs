using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultById;

public record GetIterationResultByIdQuery(Guid iterationId) : IRequest<IterationResultDto>;