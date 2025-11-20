using System;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class LeagueMapper
{

    public static Domain.Entities.League ToDomain(this LeagueDto leagueDto)
    {
        return new Domain.Entities.League
        {
            Id = leagueDto.Id,
            Name = leagueDto.Name,
            CountryId = leagueDto.CountryId,
            MaxRound = leagueDto.MaxRound,
            LeagueStrengths = leagueDto.Strengths.Select(x => ToDomain(x)).ToList()
        };
    }
    public static LeagueStrength ToDomain(this LeagueStrengthDto dto)
    {
        return new LeagueStrength
        {
            Id = dto.Id,
            LeagueId = dto.LeagueId,
            SeasonYear = EnumMapper.StringtoSeasonEnum(dto.SeasonYear),
            Strength = dto.Strength
        };
    }
}
