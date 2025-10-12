using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Interfaces;

public interface IScoreboardGrpcClient
{
    Task<List<ScoreboardDto>> GetScoreboardsByQueryAsync(Guid simulationId, CancellationToken cancellationToken, Guid iterationId = default, bool? withTeamStats = null);
}
