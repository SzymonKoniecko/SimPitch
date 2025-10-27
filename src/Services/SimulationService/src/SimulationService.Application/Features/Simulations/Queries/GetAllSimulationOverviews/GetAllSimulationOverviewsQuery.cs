using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;

public record GetAllSimulationOverviewsQuery(int offset, int limit) : IRequest<(IEnumerable<SimulationOverviewDto>, int)>;