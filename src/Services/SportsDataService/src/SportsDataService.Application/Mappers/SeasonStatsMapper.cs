using System;

namespace SportsDataService.Application.Mappers;

public static class SeasonStatsMapper
{
    public static Domain.Entities.SeasonStats ToDomain(this Application.DTOs.SeasonStatsDto dto)
    {
        if (dto == null) return null;

        return new Domain.Entities.SeasonStats
        {
            Id = dto.Id,
            TeamId = dto.TeamId,
            SeasonYear = (Domain.Enums.SeasonEnum)dto.SeasonYear,
            LeagueId = dto.LeagueId,
            MatchesPlayed = dto.MatchesPlayed,
            Wins = dto.Wins,
            Losses = dto.Losses,
            Draws = dto.Draws,
            GoalsFor = dto.GoalsFor,
            GoalsAgainst = dto.GoalsAgainst,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public static Application.DTOs.SeasonStatsDto ToDto(this Domain.Entities.SeasonStats entity)
    {
        if (entity == null) return null;

        return new Application.DTOs.SeasonStatsDto
        {
            Id = entity.Id,
            TeamId = entity.TeamId,
            SeasonYear = (int)entity.SeasonYear,
            LeagueId = entity.LeagueId,
            MatchesPlayed = entity.MatchesPlayed,
            Wins = entity.Wins,
            Losses = entity.Losses,
            Draws = entity.Draws,
            GoalsFor = entity.GoalsFor,
            GoalsAgainst = entity.GoalsAgainst,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
