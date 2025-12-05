using SportsDataService.Application.DTOs;
using SimPitchProtos.SportsDataService.Team;
using SimPitchProtos.SportsDataService;
using SportsDataService.Application.Teams.DTOs;

namespace SportsDataService.API.Mappers;

public static class TeamMapper
{
    public static TeamGrpc ToProto(this TeamDto team)
    {
        var teamGrpc = new TeamGrpc
        {
            Id = team.Id.ToString(),
            Name = team.Name,
            Country = CountryMapper.ToProto(team.Country),
            Stadium = StadiumMapper.ToProto(team.Stadium),
            ShortName = team.ShortName
        };
        teamGrpc.Memberships.AddRange(team.Memberships.Select(x => ToProto(x)));
        return teamGrpc;
    }
    public static CreateTeamDto ToDto(this CreateTeamRequest request)
    {
        return new CreateTeamDto
        {
            Name = request.Name,
            CountryId = Guid.Parse(request.CountryId),
            StadiumId = Guid.Parse(request.StadiumId),
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
    public static CompetitionMembershipGrpc ToProto(CompetitionMembershipDto dto)
    {
        var proto = new CompetitionMembershipGrpc();

        proto.Id = dto.Id.ToString();
        proto.TeamId = dto.TeamId.ToString();
        proto.LeagueId = dto.LeagueId.ToString();
        proto.SeasonYear = dto.SeasonYear;

        return proto;
    }
}