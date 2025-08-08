using SportsDataService.Domain.Entities;
using SportsDataService.SimPitchProtos;


public static class TeamMapper
{
    public static TeamResponse TeamToResponse(Team team)
    {
        return new TeamResponse
        {
            Id = team.Id.ToString(),
            Name = team.Name,
            CityId = team.CityId.ToString(),
            CountryId = team.CountryId.ToString(),
            StadiumId = team.StadiumId.ToString(),
            LeagueId = team.LeagueId.ToString(),
            LogoUrl = team.LogoUrl,
            ShortName = team.ShortName
        };
    }
}