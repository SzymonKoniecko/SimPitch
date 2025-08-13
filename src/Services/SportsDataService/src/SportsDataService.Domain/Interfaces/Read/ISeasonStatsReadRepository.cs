using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ISeasonStatsReadRepository
{
    Task<SeasonStats> GetSeasonStatsByIdAsync(Guid seasonId, CancellationToken cancellationToken);
    Task<IEnumerable<SeasonStats>> GetAllSeasonStatsAsync(CancellationToken cancellationToken);
}
