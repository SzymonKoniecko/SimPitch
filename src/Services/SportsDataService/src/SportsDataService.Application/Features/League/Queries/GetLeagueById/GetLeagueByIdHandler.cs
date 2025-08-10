using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.League.Queries.GetLeagueById;

public class GetLeagueByIdHandler
{
    private readonly ILeagueReadRepository _leagueRepository;

    public GetLeagueByIdHandler(ILeagueReadRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<LeagueDto> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
    {
        var league = await _leagueRepository.GetLeagueByIdAsync(request.LeagueId, cancellationToken);
        if (league is null)
        {
            throw new KeyNotFoundException($"League with Id '{request.LeagueId}' was not found.");
        }
        return LeagueMapper.ToDto(league);
    }
}
