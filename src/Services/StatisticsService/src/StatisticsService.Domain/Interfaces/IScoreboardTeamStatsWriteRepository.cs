using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardTeamStatsWriteRepository
{
    Task CreateScoreboardTeamStatsBulkAsync(IEnumerable<ScoreboardTeamStats> teamStatsList, CancellationToken cancellationToken);
}
