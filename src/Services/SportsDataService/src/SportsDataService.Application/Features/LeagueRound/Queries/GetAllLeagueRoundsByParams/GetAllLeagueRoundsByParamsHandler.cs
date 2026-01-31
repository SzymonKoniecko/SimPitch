using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Application.Mappers;

namespace SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

public class GetAllLeagueRoundsByParamsHandler : IRequestHandler<GetAllLeagueRoundsByParamsQuery, IEnumerable<LeagueRoundDto>>
{
    private readonly ILeagueRoundReadRepository _leagueRoundReadRepository;
    public GetAllLeagueRoundsByParamsHandler(ILeagueRoundReadRepository leagueRoundReadRepository)
    {
        _leagueRoundReadRepository = leagueRoundReadRepository;
    }
    public async Task<IEnumerable<LeagueRoundDto>> Handle(GetAllLeagueRoundsByParamsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LeagueRoundDto> leagueRoundDtos = new List<LeagueRoundDto>();

        if (request.leagueRoundFilterDto.SeasonYear != String.Empty)
        {
            leagueRoundDtos = LeagueRoundMapper.ListToDtos(
                await _leagueRoundReadRepository.GetLeagueRoundsBySeasonYearAsync(request.leagueRoundFilterDto.SeasonYear, cancellationToken
            ));
        }
        else
        {
            leagueRoundDtos = LeagueRoundMapper.ListToDtos(
                await _leagueRoundReadRepository.GetLeagueRoundsByLeagueIdAsync(request.leagueRoundFilterDto.LeagueId, cancellationToken
            ));
        }

        return leagueRoundDtos.Where(r =>
            (r.LeagueId == request.leagueRoundFilterDto.LeagueId) &&
            (request.leagueRoundFilterDto.LeagueRoundId == Guid.Empty || r.LeagueId == request.leagueRoundFilterDto.LeagueRoundId)
        );
    }
}
