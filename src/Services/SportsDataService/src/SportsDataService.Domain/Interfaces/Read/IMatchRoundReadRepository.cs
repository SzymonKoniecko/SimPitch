using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface IMatchRoundReadRepository
{
    Task<IEnumerable<MatchRound>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken);
}
