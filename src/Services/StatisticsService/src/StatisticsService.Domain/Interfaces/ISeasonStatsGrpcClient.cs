using System;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Domain.Interfaces;

public interface ISeasonStatsGrpcClient
{
    Task<List<SeasonStats>> GetSeasonStatsByLeagueAndSeasonYear(Guid leagueId, string seasonYear, CancellationToken cancellationToken);
}
