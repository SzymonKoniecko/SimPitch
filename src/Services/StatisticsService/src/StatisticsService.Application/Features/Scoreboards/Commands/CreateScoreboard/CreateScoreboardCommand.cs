using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public record CreateScoreboardCommand(Guid simulationId, Guid iterationResultId) : IRequest<IEnumerable<ScoreboardDto>>;