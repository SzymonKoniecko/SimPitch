using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface IRealMatchResultReadRepository
{
    Task<List<RealMatchResult>> GetRealMatchResultsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken);
}
