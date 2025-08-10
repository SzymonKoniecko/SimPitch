using System;

namespace SportsDataService.Application.Mappers;

public static class CountryMapper
{
    public static Domain.Entities.Country ToDomain(this Application.DTOs.CountryDto dto)
    {
        if (dto == null) return null;

        return new Domain.Entities.Country
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static Application.DTOs.CountryDto ToDto(this Domain.Entities.Country entity)
    {
        if (entity == null) return null;

        return new Application.DTOs.CountryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

}
