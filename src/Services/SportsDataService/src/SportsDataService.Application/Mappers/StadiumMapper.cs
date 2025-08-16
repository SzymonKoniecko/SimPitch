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
            Capacity = dto.Capacity
        };
    }

    public static StadiumDto ToDto(this Stadium entity)
    {
        if (entity == null) return null;

        return new StadiumDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Capacity = entity.Capacity
        };
    }
}
