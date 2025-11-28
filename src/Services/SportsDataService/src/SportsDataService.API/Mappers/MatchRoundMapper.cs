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
        grpc.IsPlayed = dto.IsPlayed;

        if (dto.HomeGoals != null)
            grpc.HomeGoals = (int)dto.HomeGoals;
        if (dto.AwayGoals != null)
            grpc.AwayGoals = (int)dto.AwayGoals;
        if (dto.IsDraw != null)
            grpc.IsDraw = (bool)dto.IsDraw;
            

        return grpc;
    }
}
