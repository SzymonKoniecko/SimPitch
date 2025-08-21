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

}
