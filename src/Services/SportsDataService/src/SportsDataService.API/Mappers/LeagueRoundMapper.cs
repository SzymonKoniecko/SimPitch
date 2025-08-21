using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.LeagueRound;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.LeagueRound.DTOs;
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

    public static LeagueRoundFilterDto LeagueRoundProtoRequestToDto(LeagueRoundsByParamsRequest request)
    {
        var dto = new LeagueRoundFilterDto();
        dto.SeasonYear = request.SeasonYear;
        dto.Round = request.HasRound ? request.Round : 0;
        dto.LeagueRoundId = request.HasLeagueRoundId ? Guid.Parse(request.LeagueRoundId) : Guid.Empty;

        return dto;
    }
}
