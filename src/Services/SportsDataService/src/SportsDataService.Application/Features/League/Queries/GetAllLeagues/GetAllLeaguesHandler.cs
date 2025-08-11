using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.League.Queries.GetAllLeagues;

public class GetAllLeaguesHandler : IRequestHandler<GetAllLeaguesQuery, IEnumerable<LeagueDto>>
{
    private readonly ILeagueReadRepository _leagueRepository;

    public GetAllLeaguesHandler(ILeagueReadRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<IEnumerable<LeagueDto>> Handle(GetAllLeaguesQuery request, CancellationToken cancellationToken)
    {
        var leagues = await _leagueRepository.GetAllLeaguesAsync(cancellationToken);
        return leagues.Select(l => LeagueMapper.ToDto(l));
    }
}