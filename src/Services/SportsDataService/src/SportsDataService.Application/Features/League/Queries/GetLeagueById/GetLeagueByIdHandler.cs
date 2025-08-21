using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.League.Queries.GetLeagueById;

public class GetLeagueByIdHandler : IRequestHandler<GetLeagueByIdQuery, LeagueDto>
{
    private readonly ILeagueReadRepository _leagueRepository;

    public GetLeagueByIdHandler(ILeagueReadRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<LeagueDto> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
    {
        var league = await _leagueRepository.GetByIdAsync(request.leagueId, cancellationToken);
        return LeagueMapper.ToDto(league);
    }
}
