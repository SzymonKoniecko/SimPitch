using System;
using SimulationService.Application.Features.Leagues.DTOs;

namespace SimulationService.Application.Mappers;

public static class LeagueMapper
{
    public static LeagueDto ToDto(this Domain.Entities.League league)
    {
        return new LeagueDto
        {
            Id = league.Id,
            Name = league.Name,
            CountryId = league.CountryId,
            MaxRound = league.MaxRound,
            Strength = league.Strength
        };
    }

    public static Domain.Entities.League ToDomain(this LeagueDto leagueDto)
    {
        return new Domain.Entities.League
        {
            Id = leagueDto.Id,
            Name = leagueDto.Name,
            CountryId = leagueDto.CountryId,
            MaxRound = leagueDto.MaxRound,
            Strength = leagueDto.Strength
        };
    }
}
