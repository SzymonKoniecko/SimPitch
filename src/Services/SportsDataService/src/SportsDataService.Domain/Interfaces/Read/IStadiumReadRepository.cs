using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface IStadiumReadRepository
{
    Task<Stadium> GetStadiumByIdAsync(Guid stadiumId, CancellationToken cancellationToken);
    Task<IEnumerable<Stadium>> GetAllStadiumsAsync(CancellationToken cancellationToken);
    Task<bool> StadiumExistsAsync(Guid stadiumId, CancellationToken cancellationToken);
}
