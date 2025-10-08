using System;
using MediatR;
using EngineService.Application.DTOs;

namespace EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

public record GetScoreboardsBySimulationIdQuery(Guid simulationId, Guid iterationResultId, bool withTeamStats) : IRequest<List<ScoreboardDto>>;