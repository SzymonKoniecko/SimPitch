using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class LeagueMapper
{

    public static Application.DTOs.LeagueDto ToDto(this Domain.Entities.League entity)
    {
        if (entity == null) return null;

        return new Application.DTOs.LeagueDto
        {
            Id = entity.Id,
            Name = entity.Name,
            MaxRound = entity.MaxRound,
            CountryId = entity.CountryId,
            Strengths = entity.Strengths.Select(x=> ToDto(x)).ToList()
        };
    }

    public static LeagueStrengthDto ToDto(LeagueStrength entity)
    {
        if (entity == null) return null;
        
        var dto = new LeagueStrengthDto();

        dto.Id = entity.Id;
        dto.LeagueId = entity.LeagueId;
        dto.SeasonYear = EnumMapper.StringtoSeasonEnum(entity.SeasonYear);
        dto.Strength = entity.Strength;

        return dto;
    }
}
