using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardReadRepository
{
    Task<IEnumerable<Scoreboard>> GetScoreboardBySimulationIdAsync(Guid simulationId, bool withTeamStats, CancellationToken cancellationToken);
    Task<bool> ScoreboardBySimulationIdExistsAsync(Guid simulationId, CancellationToken cancellationToken);
}
