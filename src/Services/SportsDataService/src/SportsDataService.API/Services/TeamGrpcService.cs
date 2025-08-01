using Grpc.Core;
using SportsDataService.API.Protos;
using SportsDataService.Application.Interfaces;

namespace SportsDataService.API.Services;

public class TeamGrpcService : TeamService.TeamServiceBase
{
    private readonly ITeamRepository _repository;

    public TeamGrpcService(ITeamRepository repository)
    {
        _repository = repository;
    }
    public override async Task<TeamListResponse> GetAllTeamsAsync(Empty request, ServerCallContext context)
    {
        var teams = await _repository.GetAllTeamsAsync();

        var response = new TeamListResponse();
        response.Teams.AddRange(teams.Select(MapToProto));
        return response;
    }

    public override async Task<TeamResponse> GetTeamById(TeamByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var guid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));

        var team = await _repository.GetTeamByIdAsync(guid);

        if (team == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Team not found"));

        return MapToProto(team);
    }

    private static TeamResponse MapToProto(Domain.Entities.Team team) => new TeamResponse
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
