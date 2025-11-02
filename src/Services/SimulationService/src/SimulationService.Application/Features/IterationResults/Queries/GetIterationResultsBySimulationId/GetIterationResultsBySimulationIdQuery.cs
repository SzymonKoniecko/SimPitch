using System;
using MediatR;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid SimulationId, PagedRequestDto PagedRequest) : IRequest<(List<IterationResultDto>, PagedResponseDetails)>;