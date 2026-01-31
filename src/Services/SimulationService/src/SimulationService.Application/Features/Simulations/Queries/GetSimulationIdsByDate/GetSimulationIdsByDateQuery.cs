using System;
using MediatR;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationIdsByDate;

public record GetSimulationIdsByDateQuery(DateTime requestedDate) : IRequest<List<string>>;