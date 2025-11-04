using System;
using MediatR;

namespace StatisticsService.Application.Features.SimulationStats.Commands;

public record CreateSimulationStatsCommand(Guid SimulationId) : IRequest<bool>;