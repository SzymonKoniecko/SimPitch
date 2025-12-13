using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Interfaces;

public interface IRedisRegistry
{
    Task SetMatchRoundsAsync(List<MatchRound> matchRounds, CancellationToken ct = default);
    Task<List<MatchRound>?> GetMatchRoundsAsync(Guid id, CancellationToken ct = default);
}
