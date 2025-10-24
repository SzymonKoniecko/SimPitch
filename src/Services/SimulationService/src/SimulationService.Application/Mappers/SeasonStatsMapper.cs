using System;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

public static class SeasonStatsMapper
{
    public static SeasonStats DtoToValueObject(SeasonStatsDto dto, float leagueStrength)
    {
        return new SeasonStats(dto.TeamId, dto.SeasonYear, dto.LeagueId, leagueStrength, dto.MatchesPlayed, dto.Wins, dto.Losses, dto.Draws, dto.GoalsFor, dto.GoalsAgainst);
    }

    public static SeasonStatsDto VoToDto(SeasonStats valueObj)
    {
        var dto = new SeasonStatsDto();

        dto.TeamId = valueObj.TeamId;
        dto.SeasonYear = valueObj.SeasonYear;
        dto.LeagueId = valueObj.LeagueId;
        dto.LeagueStrength = valueObj.LeagueStrength;
        dto.MatchesPlayed = valueObj.MatchesPlayed;
        dto.Wins = valueObj.Wins;
        dto.Losses = valueObj.Losses;
        dto.Draws = valueObj.Draws;
        dto.GoalsFor = valueObj.GoalsFor;
        dto.GoalsAgainst = valueObj.GoalsAgainst;

        return dto;
    }
}
