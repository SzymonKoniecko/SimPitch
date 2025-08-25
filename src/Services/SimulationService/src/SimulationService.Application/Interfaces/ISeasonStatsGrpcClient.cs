using System;
using SimulationService.Application.Features.SeasonsStats.DTOs;

namespace SimulationService.Application.Interfaces;

public interface ISeasonStatsGrpcClient
{
    Task<List<SeasonStatsDto>> GetSeasonsStatsByTeamIdAsync(Guid teamId, CancellationToken cancellationToken);
}
