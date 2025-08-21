using System;
using SimulationService.Application.Features.MatchRounds.DTOs;

namespace SimulationService.Application.Interfaces;

public interface IMatchRoundGrpcClient
{
    Task<List<MatchRoundDto>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken);
}
