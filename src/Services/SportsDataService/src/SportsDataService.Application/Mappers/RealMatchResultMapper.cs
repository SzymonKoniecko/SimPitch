using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class RealMatchResultMapper
{
    public static RealMatchResultDto ToDto(RealMatchResult entity)
    {
        var dto = new RealMatchResultDto();
        dto.Id = entity.Id;
        dto.RoundId = entity.RoundId;
        dto.HomeTeamId = entity.HomeTeamId;
        dto.AwayTeamId = entity.AwayTeamId;
        dto.HomeGoals = entity.HomeGoals;
        dto.AwayGoals = entity.AwayGoals;
        dto.IsDraw = entity.IsDraw;

        return dto;
    }
    public static IEnumerable<RealMatchResultDto> ListToDtos(List<RealMatchResult> entityList)
    {
        return entityList.Select(x => ToDto(x));
    }
}
