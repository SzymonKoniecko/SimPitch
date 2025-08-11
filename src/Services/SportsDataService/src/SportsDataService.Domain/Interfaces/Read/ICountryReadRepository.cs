using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ICountryReadRepository
{
    Task<Country> GetCountryByIdAsync(Guid countryId, CancellationToken cancellationToken);
    Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken);
    Task<bool> CountryExistsAsync(Guid countryId, CancellationToken cancellationToken);
}
