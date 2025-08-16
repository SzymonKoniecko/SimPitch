using System;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.DTOs;

namespace SportsDataService.API.Mappers;

public static class SeasonStatsMapper
{
    public static SeasonStatsDto ToDto(this SportsDataService.Domain.Entities.SeasonStats entity)
    {
        return new SeasonStatsDto
        {
            Id = entity.Id,
            TeamId = entity.TeamId,
            SeasonYear = EnumMapper.StringtoSeasonEnum(entity.SeasonYear),
            LeagueId = entity.LeagueId,
            MatchesPlayed = entity.MatchesPlayed,
            Wins = entity.Wins,
            Losses = entity.Losses,
            Draws = entity.Draws,
            GoalsFor = entity.GoalsFor,
            GoalsAgainst = entity.GoalsAgainst
        };
    }

    public static SeasonStatsDto ToDto(this SeasonStatsGrpc grpc)
    {
        return new SeasonStatsDto
        {
            Id = Guid.Parse(grpc.Id),
            TeamId = Guid.Parse(grpc.TeamId),
            SeasonYear = EnumMapper.StringtoSeasonEnum(grpc.SeasonYear),
            LeagueId = Guid.Parse(grpc.LeagueId),
            MatchesPlayed = (int)grpc.MatchesPlayed,
            Wins = (int)grpc.Wins,
            Losses = (int)grpc.Losses,
            Draws = (int)grpc.Draws,
            GoalsFor = (int)grpc.GoalsFor,
            GoalsAgainst = (int)grpc.GoalsAgainst
        };
    }
    public static SeasonStatsGrpc ToProto(this SeasonStatsDto dto)
    {
        return new SeasonStatsGrpc
        {
            Id = dto.Id.ToString(),
            TeamId = dto.TeamId.ToString(),
            SeasonYear = EnumMapper.SeasonEnumToString(dto.SeasonYear),
            LeagueId = dto.LeagueId.ToString(),
            MatchesPlayed = dto.MatchesPlayed,
            Wins = dto.Wins,
            Losses = dto.Losses,
            Draws = dto.Draws,
            GoalsFor = dto.GoalsFor,
            GoalsAgainst = dto.GoalsAgainst
        };
    }
}
