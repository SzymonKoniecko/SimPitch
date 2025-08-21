using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class LeagueRoundMapper
{
    public static LeagueRoundDto ToDto(this LeagueRound enitiy)
    {
        if (enitiy == null) return null;

        var dto = new LeagueRoundDto();
        dto.Id = enitiy.Id;
        dto.LeagueId = enitiy.LeagueId;
        dto.SeasonYear = enitiy.SeasonYear;
        dto.Round = enitiy.Round;

        return dto;
    }
    public static IEnumerable<LeagueRoundDto> ListToDtos(this IEnumerable<LeagueRound> entityList)
    {
        if (entityList == null) return null;

        return entityList.Select(x => ToDto(x));
    }
}
