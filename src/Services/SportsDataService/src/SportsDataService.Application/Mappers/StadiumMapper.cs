using System;
using SportsDataService.Application.DTOs.Feature;
using SportsDataService.Application.Features.Stadium.Commands.CreateStadium;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class StadiumMapper
{
    public static Domain.Entities.Stadium ToDomain(this Application.DTOs.StadiumDto dto)
    {
        if (dto == null) return null;

        return new Domain.Entities.Stadium
        {
            Id = dto.Id,
            Name = dto.Name,
            Capacity = dto.Capacity,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static Application.DTOs.StadiumDto ToDto(this Domain.Entities.Stadium entity)
    {
        if (entity == null) return null;

        return new Application.DTOs.StadiumDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Capacity = entity.Capacity,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
