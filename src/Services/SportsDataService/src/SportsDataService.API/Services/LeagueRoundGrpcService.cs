using System;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SportsDataService.LeagueRound;
using SportsDataService.API.Mappers;
using SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

namespace SportsDataService.API.Services;

public class LeagueRoundGrpcService : LeagueRoundService.LeagueRoundServiceBase
{
    private readonly IMediator _mediator;

    public LeagueRoundGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public override async Task<LeagueRoundsByParamsResponse> GetAllLeagueRoundsByParams(LeagueRoundsByParamsRequest request, ServerCallContext context)
    {
        var query = new GetAllLeagueRoundsByParamsQuery(request.SeasonYear, request.Round, Guid.Parse(request.LeagueRoundId));
        var leagueRoundsDtos = await _mediator.Send(query, context.CancellationToken);

        return new LeagueRoundsByParamsResponse
        {
            LeagueRounds = { leagueRoundsDtos.Select(l => LeagueRoundMapper.ToProto(l))}
        };
    }
}
