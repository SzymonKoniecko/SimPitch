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
        IEnumerable<LeagueRoundDto> leagueRoundDtos = LeagueRoundMapper.ListToDtos(await _leagueRoundReadRepository.GetLeagueRoundsBySeasonYearAsync(request.seasonYear, cancellationToken));

        return leagueRoundDtos.Where(r =>
            (!request.round.HasValue || r.Round == request.round) &&
            (!request.leagueRoundId.HasValue || r.LeagueId == request.leagueRoundId)
        );
    }
}
