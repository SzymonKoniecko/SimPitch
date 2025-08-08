
using Grpc.Core;
using MediatR;
using SportsDataService.SimPitchProtos;

namespace SportsDataService.API.Services
{
    public class TeamGrpcService : TeamService.TeamServiceBase
    {
        private readonly IMediator _mediator;

        public TeamGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<TeamListResponse> GetAllTeamsAsync(Empty request, ServerCallContext context)
        {
            var teams = await _mediator.Send(new GetAllTeamsQuery());

            if (teams == null || !teams.Any())
                throw new RpcException(new Status(StatusCode.NotFound, "No teams found"));

            var response = new TeamListResponse();
            response.Teams.AddRange(teams.Select(TeamMapper.TeamToResponse));
            return response;
        }

        public override async Task<TeamResponse> GetTeamById(TeamByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var guid))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));

            var team = await _mediator.Send(new GetTeamByIdQuery(guid)); // Poprawiłem nazwę zapytania (GetTeamByIdQuery)

            if (team == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Team not found"));

            return TeamMapper.TeamToResponse(team);
        }
    }
}
