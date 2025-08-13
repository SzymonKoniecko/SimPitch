using System;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.Stadium.Commands.CreateStadium;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Mappers;

public static class StadiumMapper
{
    public static Stadium ToDomain(this StadiumDto dto)
    {
        if (dto == null) return null;

        return new Stadium
        {
            Id = dto.Id,
            Name = dto.Name,
            Capacity = dto.Capacity,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static StadiumDto ToDto(this Stadium entity)
    {
        if (entity == null) return null;

        return new StadiumDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Capacity = entity.Capacity,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
