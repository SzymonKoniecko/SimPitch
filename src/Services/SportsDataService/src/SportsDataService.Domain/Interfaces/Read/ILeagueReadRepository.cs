using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ILeagueReadRepository
{
    Task<League> GetLeagueByIdAsync(Guid leagueId, CancellationToken cancellationToken);
    Task<IEnumerable<League>> GetAllLeaguesAsync(CancellationToken cancellationToken);
    Task<bool> LeagueExistsAsync(Guid leagueId, CancellationToken cancellationToken);
}
