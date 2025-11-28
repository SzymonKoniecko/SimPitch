using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.MatchRound;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Infrastructure.Clients;

public class MatchRoundGrpcClient : IMatchRoundGrpcClient
{

    private readonly MatchRoundService.MatchRoundServiceClient _matchRoundServiceClient;

    public MatchRoundGrpcClient(MatchRoundService.MatchRoundServiceClient matchRoundServiceClient)
    {
        _matchRoundServiceClient = matchRoundServiceClient;
    }

    public async Task<List<MatchRoundDto>> GetMatchRoundsByRoundIdAsync(Guid roundId, CancellationToken cancellationToken)
    {
        var grpcRequest = new MatchRoundsByRoundIdRequest();
        grpcRequest.RoundId = roundId.ToString();

        var response = await _matchRoundServiceClient.GetMatchRoundsByRoundIdAsync(grpcRequest, cancellationToken: cancellationToken);

        var result = new List<MatchRoundDto>();
        result.AddRange(response.MatchRounds?.Select(mr => ProtoToDto(mr)));

        return result;
    }

    private static MatchRoundDto ProtoToDto(MatchRoundGrpc grpc) {

        var dto = new MatchRoundDto();
        dto.Id = Guid.Parse(grpc.Id);
        dto.RoundId = Guid.Parse(grpc.RoundId);
        dto.HomeTeamId = Guid.Parse(grpc.HomeTeamId);
        dto.AwayTeamId = Guid.Parse(grpc.AwayTeamId);
        dto.IsPlayed = grpc.IsPlayed;
        
        dto.HomeGoals = grpc.HasHomeGoals ? grpc.HomeGoals : null;
        dto.AwayGoals = grpc.HasAwayGoals ? grpc.AwayGoals : null;
        dto.IsDraw = grpc.HasIsDraw ? grpc.IsDraw : null;

        return dto;
    }
}
