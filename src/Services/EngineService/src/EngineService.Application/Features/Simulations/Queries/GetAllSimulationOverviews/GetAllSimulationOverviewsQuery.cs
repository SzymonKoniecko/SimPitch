using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;

public record GetAllSimulationOverviewsQuery(PagedRequest PagedRequest) : IRequest<PagedResponse<SimulationOverviewDto>>;
