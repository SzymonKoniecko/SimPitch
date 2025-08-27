using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardTeamStatsReadRepository
{
    Task<IEnumerable<ScoreboardTeamStats>> GetScoreboardByScoreboardIdAsync(Guid scoreboardId, CancellationToken cancellationToken);
}
