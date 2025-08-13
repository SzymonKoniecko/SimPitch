using SimulationService.Application.Features.LeagueRounds.DTOs;

namespace SimulationService.Application.Interfaces;

public interface ILeagueRoundGrpcClient
{
    public Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(LeagueRoundDtoRequest request, CancellationToken cancellationToken);
}
