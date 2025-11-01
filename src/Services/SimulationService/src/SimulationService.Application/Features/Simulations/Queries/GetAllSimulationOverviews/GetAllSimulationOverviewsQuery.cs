using System;
using MediatR;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;

public record GetAllSimulationOverviewsQuery(PagedRequestDto PagedRequest) : IRequest<(IEnumerable<SimulationOverviewDto>, PagedResponseDetails)>;