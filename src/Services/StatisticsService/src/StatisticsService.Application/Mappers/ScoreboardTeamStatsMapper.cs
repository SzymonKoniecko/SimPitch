using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class ScoreboardTeamStatsMapper
{
    public static ScoreboardTeamStatsDto ToDto(ScoreboardTeamStats entity)
    {
        var dto = new ScoreboardTeamStatsDto();

        dto.Id = entity.Id;
        dto.ScoreboardId = entity.ScoreboardId;
        dto.TeamId = entity.TeamId;
        dto.Rank = entity.Rank;
        dto.Points = entity.Points;
        dto.MatchPlayed = entity.MatchPlayed;
        dto.Wins = entity.Wins;
        dto.Losses = entity.Losses;
        dto.Draws = entity.Draws;
        dto.GoalsFor = entity.GoalsFor;
        dto.GoalsAgainst = entity.GoalsAgainst;

        return dto;
    }
}
