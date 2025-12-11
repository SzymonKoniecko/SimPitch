using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardReadRepository
{
    Task<IEnumerable<Scoreboard>> GetScoreboardByQueryAsync(Guid simulationId, Guid iterationResultId, bool withTeamStats, CancellationToken cancellationToken);
    Task<bool> ScoreboardsBySimulationIdExistsAsync(Guid simulationId, int expectedScoreboards, CancellationToken cancellationToken);
    Task<bool> ScoreboardByIterationResultIdExistsAsync(Guid iterationResultId, CancellationToken cancellationToken);
}
