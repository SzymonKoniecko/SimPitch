using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;

public class GetLeagueRoundsByParamsGrpcHandler : IRequestHandler<GetLeagueRoundsByParamsGrpcQuery, List<LeagueRoundDto>>
{
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;

    public GetLeagueRoundsByParamsGrpcHandler(ILeagueRoundGrpcClient leagueRoundGrpcClient)
    {
        _leagueRoundGrpcClient = leagueRoundGrpcClient;
    }

    public async Task<List<LeagueRoundDto>> Handle
    (
        GetLeagueRoundsByParamsGrpcQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _leagueRoundGrpcClient
            .GetAllLeagueRoundsByParams(request.seasonYear, request.round, request.leagueRoundId, cancellationToken);
    }
}
