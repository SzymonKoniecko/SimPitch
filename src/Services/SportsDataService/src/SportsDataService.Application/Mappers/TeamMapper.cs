using SportsDataService.Application.DTOs;
using SportsDataService.Application.Teams.DTOs;
using SportsDataService.Domain.Entities;
namespace SportsDataService.Application.Mappers;

public static class TeamMapper
{
    public static Team ToDomain(CreateTeamDto dto)
    {
        if (dto == null) return null;

        return new Team
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CountryId = dto.CountryId,
            StadiumId = dto.StadiumId,
            ShortName = dto.ShortName
        };
    }
    public static TeamDto ToDto(Team team, Country country, Stadium stadium, IEnumerable<CompetitionMembership> competitionMemberships)
    {
        if (team == null) return null;

        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Country = country?.ToDto(),
            Stadium = stadium?.ToDto(),
            Memberships = competitionMemberships.Select(x => ToDto(x)).ToList(),
            ShortName = team.ShortName
        };
    }

    private static CompetitionMembershipDto ToDto(CompetitionMembership competitionMemberships)
    {
        var dto = new CompetitionMembershipDto();

        dto.Id = competitionMemberships.Id;
        dto.TeamId = competitionMemberships.TeamId;
        dto.LeagueId = competitionMemberships.LeagueId;
        dto.SeasonYear = competitionMemberships.SeasonYear;

        return dto;
    }
}