using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.LeagueRound;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Infrastructure.Clients;

public class LeagueRoundGrpcClient : ILeagueRoundGrpcClient
{
    private readonly LeagueRoundService.LeagueRoundServiceClient _leagueRoundClient;

    public LeagueRoundGrpcClient(LeagueRoundService.LeagueRoundServiceClient leagueRoundClient)
    {
        _leagueRoundClient = leagueRoundClient;
    }

    public async Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(LeagueRoundDtoRequest request, CancellationToken cancellationToken)
    {
    var grpcRequest = new LeagueRoundsByParamsRequest
    {
        SeasonYear = request.SeasonYear
    };
    
    if (request.Round != 0)
            grpcRequest.Round = request.Round;
    if (request.LeagueRoundId != Guid.Empty)
        grpcRequest.LeagueRoundId = request.LeagueRoundId.ToString();

        var response = await _leagueRoundClient.GetAllLeagueRoundsByParamsAsync(grpcRequest, cancellationToken: cancellationToken);

        var result = new List<LeagueRoundDto>();
        result.AddRange(response.LeagueRounds?.Select(lr => ProtoToDto(lr)));

        return result;
    }

    private static LeagueRoundDto ProtoToDto(LeagueRoundGrpc grpc)
    {
        var dto = new LeagueRoundDto();
        dto.Id = Guid.Parse(grpc.Id);
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.SeasonYear = grpc.SeasonYear;
        dto.Round = grpc.Round;

        return dto;
    }
}
