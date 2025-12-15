using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces;

public interface IRedisRegistry
{
    Task SetMatchRoundsAsync(IEnumerable<MatchRound> matchRounds, CancellationToken ct = default);
    Task<IEnumerable<MatchRound>?> GetMatchRoundsAsync(CancellationToken ct = default);
}
