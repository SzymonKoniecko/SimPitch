using StatisticsService.Application.Features.LeagueRounds.DTOs;

namespace StatisticsService.Application.Interfaces;

public interface ILeagueRoundGrpcClient
{
    public Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(LeagueRoundDtoRequest request, CancellationToken cancellationToken);
}
