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

    public async Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(string seasonYear, int? round, Guid? leagueRoundId, CancellationToken cancellationToken)
    {
        var request = new LeagueRoundsByParamsRequest();
        request.SeasonYear = seasonYear;
        request.Round = round ?? 0;
        request.LeagueRoundId = leagueRoundId.ToString() ?? "";

        var response = await _leagueRoundClient.GetAllLeagueRoundsByParamsAsync(request, cancellationToken: cancellationToken);

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
