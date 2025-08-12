using System;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.API.Mappers;

public static class LeagueRoundMapper
{
    public static LeagueRoundGrpc ToProto(this LeagueRoundDto dto)
    {
        if (dto == null) return null;

        var grpc = new LeagueRoundGrpc();
        grpc.Id = dto.Id.ToString();
        grpc.LeagueId = dto.LeagueId.ToString();
        grpc.SeasonYear = dto.SeasonYear;
        grpc.Round = dto.Round;

        return grpc;
    }
}
