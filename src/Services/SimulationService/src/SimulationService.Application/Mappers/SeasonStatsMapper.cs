using System;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

public static class SeasonStatsMapper
{
    public static SeasonStats DtoToValueObject(SeasonStatsDto dto)
    {
        return new SeasonStats(dto.TeamId, dto.SeasonYear, dto.LeagueId, dto.MatchesPlayed, dto.Wins, dto.Losses, dto.Draws, dto.GoalsFor, dto.GoalsAgainst);
    }
}
