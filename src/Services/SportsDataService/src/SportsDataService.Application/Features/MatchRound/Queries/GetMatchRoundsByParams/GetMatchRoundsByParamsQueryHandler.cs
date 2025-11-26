using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.League.Queries.GetLeagueById;
using SportsDataService.Application.Features.LeagueRound.DTOs;
using SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;
using SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;

namespace SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByParams;

public class GetMatchRoundsByParamsQueryHandler : IRequestHandler<GetMatchRoundsByParamsQuery, List<MatchRoundDto>>
{
    private readonly IMediator _mediator;

    public GetMatchRoundsByParamsQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<MatchRoundDto>> Handle(GetMatchRoundsByParamsQuery query, CancellationToken cancellationToken)
    {
        List<MatchRoundDto> matchRoundDtos = new();

        var leagueRounds = await _mediator.Send(
            new GetAllLeagueRoundsByParamsQuery(
                new LeagueRoundFilterDto(){ LeagueId = query.LeagueId, SeasonYear = query.seasonYear}), 
            cancellationToken
        );

        foreach (var lr in leagueRounds)
        {
            matchRoundDtos.AddRange(await _mediator.Send(new GetMatchRoundsByRoundIdQuery(lr.Id), cancellationToken));
        }

        return matchRoundDtos;
    }
}
