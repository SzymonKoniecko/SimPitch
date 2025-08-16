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
    private readonly ILogger<LeagueRoundGrpcService> _logger;

    public LeagueRoundGrpcService(IMediator mediator, ILogger<LeagueRoundGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public override async Task<LeagueRoundsByParamsResponse> GetAllLeagueRoundsByParams(LeagueRoundsByParamsRequest request, ServerCallContext context)
    {
        var query = new GetAllLeagueRoundsByParamsQuery(LeagueRoundMapper.LeagueRoundProtoRequestToDto(request));
        var leagueRoundsDtos = await _mediator.Send(query, context.CancellationToken);

        return new LeagueRoundsByParamsResponse
        {
            LeagueRounds = { leagueRoundsDtos.Select(l => LeagueRoundMapper.ToProto(l))}
        };
    }
}
