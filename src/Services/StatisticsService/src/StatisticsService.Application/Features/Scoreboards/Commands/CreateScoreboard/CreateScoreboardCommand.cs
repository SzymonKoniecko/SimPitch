using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public record CreateScoreboardCommand(SimulationResultDto simulationimulationResultDto) : IRequest<Guid>;