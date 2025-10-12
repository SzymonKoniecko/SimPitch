using System;
using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

public record GetScoreboardsBySimulationIdQuery(Guid simulationId, Guid iterationResultId, bool withTeamStats) : IRequest<List<ScoreboardDto>>;