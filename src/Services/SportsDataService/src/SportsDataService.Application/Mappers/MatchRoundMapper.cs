using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class MatchRoundMapper
{
    public static MatchRoundDto ToDto(MatchRound entity)
    {
        var dto = new MatchRoundDto();
        dto.Id = entity.Id;
        dto.RoundId = entity.RoundId;
        dto.HomeTeamId = entity.HomeTeamId;
        dto.AwayTeamId = entity.AwayTeamId;
        dto.HomeGoals = entity.HomeGoals;
        dto.AwayGoals = entity.AwayGoals;
        dto.IsDraw = entity.IsDraw;
        dto.IsPlayed = entity.IsPlayed;

        return dto;
    }
    public static IEnumerable<MatchRoundDto> ListToDtos(IEnumerable<MatchRound> entityList)
    {
        return entityList.Select(x => ToDto(x));
    }
}
