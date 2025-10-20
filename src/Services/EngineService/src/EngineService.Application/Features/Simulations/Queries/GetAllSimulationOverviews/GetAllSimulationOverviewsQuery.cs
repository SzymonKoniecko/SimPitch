using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;

public record GetAllSimulationOverviewsQuery() : IRequest<List<SimulationOverviewDto>>;
