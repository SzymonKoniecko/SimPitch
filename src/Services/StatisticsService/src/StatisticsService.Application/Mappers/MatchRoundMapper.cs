using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class MatchRoundMapper
{
    public static MatchRound ToValueObject(MatchRoundDto dto)
    {
        var valueObject = new MatchRound();

        valueObject.Id = dto.Id;
        valueObject.RoundId = dto.RoundId;
        valueObject.HomeTeamId = dto.HomeTeamId;
        valueObject.AwayTeamId = dto.AwayTeamId;
        valueObject.HomeGoals = dto.HomeGoals;
        valueObject.AwayGoals = dto.AwayGoals;
        valueObject.IsDraw = dto.IsDraw;
        valueObject.IsPlayed = dto.IsPlayed;

        return valueObject;
    }
}
