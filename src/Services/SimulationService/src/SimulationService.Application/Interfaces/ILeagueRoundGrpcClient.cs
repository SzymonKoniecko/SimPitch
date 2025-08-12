using SimulationService.Application.Features.LeagueRounds.DTOs;

namespace SimulationService.Application.Interfaces;

public interface ILeagueRoundGrpcClient
{
    public Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(string seasonYear, int? round, Guid? leagueRoundId, CancellationToken cancellationToken);
}
