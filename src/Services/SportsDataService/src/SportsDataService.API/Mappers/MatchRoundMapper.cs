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
        dto.Id = dto.Id;
        dto.RoundId = dto.RoundId;
        dto.HomeTeamId = dto.HomeTeamId;
        dto.AwayTeamId = dto.AwayTeamId;
        dto.HomeGoals = dto.HomeGoals;
        dto.AwayGoals = dto.AwayGoals;
        dto.IsDraw = dto.IsDraw;

        return grpc;
    }
}
