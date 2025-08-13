using SportsDataService.Application.DTOs;
using SportsDataService.Application.DTOs.Feature;
using SportsDataService.Domain.Entities;
namespace SportsDataService.Application.Mappers;

public static class TeamMapper
{
    public static Team ToDomain(this TeamDto dto)
    {
        if (dto == null) return null;

        return new Team
        {
            Id = dto.Id,
            Name = dto.Name,
            CountryId = dto.Country.Id,
            StadiumId = dto.Stadium.Id,
            LeagueId = dto.League.Id,
            LogoUrl = dto.LogoUrl,
            ShortName = dto.ShortName,
            Sport = dto.Sport
        };
    }

    public static Team ToDomain(CreateTeamDto dto)
    {
        if (dto == null) return null;

        return new Team
        {
            Name = dto.Name,
            CountryId = dto.CountryId,
            StadiumId = dto.StadiumId,
            LeagueId = dto.LeagueId,
            LogoUrl = dto.LogoUrl,
            ShortName = dto.ShortName
        };
    }
    public static TeamDto ToDto(Team team, Country country, Stadium stadium, League league)
    {
        if (team == null) return null;

        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Country = country?.ToDto(),
            Stadium = stadium?.ToDto(),
            League = league?.ToDto(),
            LogoUrl = team.LogoUrl,
            ShortName = team.ShortName
        };
    }
    
}