using System;
using MediatR;
using EngineService.Application.DTOs;
using EngineService.Application.Common.Pagination;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid simulationId, int pageNumber, int pageSize) : IRequest<PagedResponse<IterationResultDto>>;