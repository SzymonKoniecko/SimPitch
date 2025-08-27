using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardTeamStatsWriteRepository
{
    Task CreateScoreboardTeamStatsAsync(ScoreboardTeamStats teamStats, CancellationToken cancellationToken);
    Task CreateScoreboardTeamStatsBulkAsync(IEnumerable<ScoreboardTeamStats> teamStatsList, CancellationToken cancellationToken);
}
