using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ISeasonStatsReadRepository
{
    Task<IEnumerable<SeasonStats>> GetSeasonsStatsByTeamIdAsync(Guid teamId, CancellationToken cancellationToken);
    Task<IEnumerable<SeasonStats>> GetAllSeasonStatsAsync(CancellationToken cancellationToken);
}
