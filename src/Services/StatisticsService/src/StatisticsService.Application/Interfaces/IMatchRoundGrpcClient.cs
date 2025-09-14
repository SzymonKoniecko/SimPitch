using System;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Interfaces;

public interface IMatchRoundGrpcClient
{
    Task<List<MatchRoundDto>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken);
}
