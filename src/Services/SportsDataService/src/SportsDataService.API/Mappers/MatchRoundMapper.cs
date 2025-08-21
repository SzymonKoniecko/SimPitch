using System;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.API.Mappers;

public static class MatchRoundMapper
{
    public static MatchRoundGrpc ToProto(MatchRoundDto dto)
    {
        var grpc = new MatchRoundGrpc();
        grpc.Id = dto.Id.ToString();
        grpc.RoundId = dto.RoundId.ToString();
        grpc.HomeTeamId = dto.HomeTeamId.ToString();
        grpc.AwayTeamId = dto.AwayTeamId.ToString();
        grpc.HomeGoals = dto.HomeGoals;
        grpc.AwayGoals = dto.AwayGoals;
        grpc.IsDraw = dto.IsDraw;
        grpc.IsPlayed = dto.IsPlayed;

        return grpc;
    }
}
