using System;

namespace SportsDataService.Application.Mappers;

public static class LeagueMapper
{
    public static Domain.Entities.League ToDomain(this Application.DTOs.LeagueDto dto)
    {
        if (dto == null) return null;

        return new Domain.Entities.League
        {
            Id = dto.Id,
            Name = dto.Name,
            MaxRound = dto.MaxRound,
            CountryId = dto.CountryId,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static Application.DTOs.LeagueDto ToDto(this Domain.Entities.League entity)
    {
        if (entity == null) return null;

        return new Application.DTOs.LeagueDto
        {
            Id = entity.Id,
            Name = entity.Name,
            MaxRound = entity.MaxRound,
            CountryId = entity.CountryId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
