using System;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SportsDataService.LeagueRound;
using SportsDataService.API.Mappers;
using SportsDataService.Application.Features.LeagueRound.DTOs;
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
        var leagueRoundFilterDto = new LeagueRoundFilterDto();
        leagueRoundFilterDto.SeasonYear = request.SeasonYear;
        leagueRoundFilterDto.Round = request.Round;
        leagueRoundFilterDto.LeagueRoundId = Guid.Parse(request.LeagueRoundId);

        var query = new GetAllLeagueRoundsByParamsQuery(leagueRoundFilterDto);
        var leagueRoundsDtos = await _mediator.Send(query, context.CancellationToken);

        return new LeagueRoundsByParamsResponse
        {
            LeagueRounds = { leagueRoundsDtos.Select(l => LeagueRoundMapper.ToProto(l))}
        };
    }
}
