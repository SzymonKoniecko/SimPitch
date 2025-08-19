using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ILeagueReadRepository
{
    Task<IEnumerable<League>> GetLeaguesByCountryIdAsync(Guid countryId, CancellationToken cancellationToken);
    Task<IEnumerable<League>> GetAllLeaguesAsync(CancellationToken cancellationToken);
    Task<bool> LeagueExistsAsync(Guid leagueId, CancellationToken cancellationToken);
    Task<League> GetByIdAsync(Guid leagueId, CancellationToken cancellationToken);
}
