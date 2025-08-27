using System;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class MatchRoundMapper
{
    public static MatchRound ToDomain(MatchRoundDto dto)
    {
        var entity = new MatchRound();
        entity.Id = dto.Id;
        entity.RoundId = dto.RoundId;
        entity.HomeTeamId = dto.HomeTeamId;
        entity.AwayTeamId = dto.AwayTeamId;
        entity.HomeGoals = dto.HomeGoals;
        entity.AwayGoals = dto.AwayGoals;
        entity.IsDraw = dto.IsDraw;
        entity.IsPlayed = dto.IsPlayed;

        return entity;
    }

    public static IEnumerable<MatchRound> ToDomainBulk(IEnumerable<MatchRoundDto> dtos)
    {
        return dtos.Select(ToDomain).ToList();
    }

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

    public static IEnumerable<MatchRoundDto> ToDtoBulk(IEnumerable<MatchRound> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}
