using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface IFootballSeasonStatsReadRepository
{
    Task<FootballSeasonStats> GetSeasonStatsByIdAsync(Guid seasonId, CancellationToken cancellationToken);
    Task<IEnumerable<FootballSeasonStats>> GetAllSeasonStatsAsync(CancellationToken cancellationToken);
}
