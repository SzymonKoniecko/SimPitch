using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Interfaces;

public interface IScoreboardDataService
{
    Task<IEnumerable<IterationResult>> GetIterationResultsAsync(Guid simulationId, CancellationToken ct);
    Task<List<MatchRoundDto>> GetPlayedMatchRoundsAsync(SimulationOverview overview, CancellationToken ct);
}
