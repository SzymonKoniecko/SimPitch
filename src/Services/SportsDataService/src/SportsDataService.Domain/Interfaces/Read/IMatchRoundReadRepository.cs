using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface IMatchRoundReadRepository
{
    Task<List<MatchRound>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken);
}
