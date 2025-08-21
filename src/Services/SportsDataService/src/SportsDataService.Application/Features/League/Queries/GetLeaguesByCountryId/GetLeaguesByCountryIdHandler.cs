using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.League.Queries.GetLeagueById;

public class GetLeaguesByCountryIdHandler : IRequestHandler<GetLeaguesByCountryIdQuery, IEnumerable<LeagueDto>> 
{
    private readonly ILeagueReadRepository _leagueRepository;

    public GetLeaguesByCountryIdHandler(ILeagueReadRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<IEnumerable<LeagueDto>> Handle(GetLeaguesByCountryIdQuery request, CancellationToken cancellationToken)
    {
        var leagues = await _leagueRepository.GetLeaguesByCountryIdAsync(request.CountryId, cancellationToken);
        if (leagues is null)
        {
            throw new KeyNotFoundException($"League with Id '{request.CountryId}' was not found.");
        }
        return leagues.Select(l => LeagueMapper.ToDto(l));
    }
}
