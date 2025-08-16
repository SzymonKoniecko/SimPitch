using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;

public class GetLeagueRoundsByParamsGrpcHandler : IRequestHandler<GetLeagueRoundsByParamsGrpcQuery, List<LeagueRound>>
{
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;

    public GetLeagueRoundsByParamsGrpcHandler(ILeagueRoundGrpcClient leagueRoundGrpcClient)
    {
        _leagueRoundGrpcClient = leagueRoundGrpcClient;
    }

    public async Task<List<LeagueRound>> Handle
    (
        GetLeagueRoundsByParamsGrpcQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _leagueRoundGrpcClient.GetAllLeagueRoundsByParams(request.leagueRoundDtoRequest, cancellationToken);
        return result.Select(x => LeagueRoundMapper.ToDomain(x)).ToList();
    }
}
