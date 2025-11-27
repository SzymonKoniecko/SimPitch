using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.MatchRound;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;


namespace StatisticsService.Infrastructure.Clients;
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
        dto.HomeGoals = grpc.AwayGoals;
        dto.IsDraw = grpc.IsDraw;
        dto.IsPlayed = grpc.IsPlayed;

        if (dto.HomeGoals != dto.AwayGoals && dto.IsDraw)
        {
            throw new Exception($"MatchRound {dto.Id} - has isDraw=true, but goals are not equal HomeVsAway({dto.HomeGoals}:{dto.AwayGoals})");
        }
        if (dto.HomeGoals == dto.AwayGoals && dto.IsDraw == false)
        {
            throw new Exception($"MatchRound {dto.Id} - has isDraw=false, but goals are equal HomeVsAway({dto.HomeGoals}:{dto.AwayGoals})");
        }
        return dto;
    }
}
