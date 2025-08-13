using SportsDataService.Application.DTOs;
using SimPitchProtos.SportsDataService.Team;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.DTOs.Feature;

namespace SportsDataService.API.Mappers;

public static class TeamMapper
{
    public static TeamGrpc ToProto(this TeamDto team)
    {
        return new TeamGrpc
        {
            Id = team.Id.ToString(),
            Name = team.Name,
            Country = CountryMapper.ToProto(team.Country),
            Stadium = StadiumMapper.ToProto(team.Stadium),
            League = LeagueMapper.ToProto(team.League),
            LogoUrl = team.LogoUrl,
            ShortName = team.ShortName
        };
    }
    public static CreateTeamDto ToDto(this CreateTeamRequest request)
    {
        return new CreateTeamDto
        {
            Name = request.Name,
            CountryId = Guid.Parse(request.CountryId),
            StadiumId = Guid.Parse(request.StadiumId),
            LeagueId = Guid.Parse(request.LeagueId),
            LogoUrl = request.LogoUrl,
            ShortName = request.ShortName
        };
    }
    // Lists
    public static TeamListResponse ListToDto(this IEnumerable<TeamDto> teams)
    {
        var response = new TeamListResponse();
        response.Teams.AddRange(teams.Select(ToProto));
        return response;
    }
}