using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardReadRepository
{
    Task<IEnumerable<Scoreboard>> GetScoreboardBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken);
}
