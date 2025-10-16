using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Queries.GetAllSimulations;

public record GetAllSimulationsQuery() : IRequest<List<SimulationOverviewDto>>;
