
using Grpc.Core;
using MediatR;
using SimPitchProtos.SportsDataService.Team;
using SimPitchProtos.SportsDataService;
using Google.Protobuf.WellKnownTypes;
using SportsDataService.Application.DTOs;
using SportsDataService.API.Mappers;
using SportsDataService.Application.Features.Teams.Commands.CreateTeam;
using SportsDataService.Application.Features.Teams.Queries.GetTeamById;
using SportsDataService.Application.Features.Teams.Queries.GetAllTeams;

namespace SportsDataService.API.Services
{
    public class TeamGrpcService : TeamService.TeamServiceBase
    {
        private readonly IMediator _mediator;

        public TeamGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<TeamListResponse> GetAllTeams(Empty request, ServerCallContext context)
        {
            IEnumerable<TeamDto> teams = await _mediator.Send(new GetAllTeamsQuery());

            if (teams == null || !teams.Any())
                throw new RpcException(new Status(StatusCode.NotFound, "No teams found"));

            return TeamMapper.ListToDto(teams);
        }

        public override async Task<TeamGrpc> GetTeamById(TeamByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var guid))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));

            var team = await _mediator.Send(new GetTeamByIdQuery(guid));

            if (team == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Team not found"));

            return team.ToProto();
        }
        public override async Task<TeamIdResponse> CreateTeam(CreateTeamRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Request cannot be null"));
                
            var teamId = await _mediator.Send(new CreateTeamCommand(TeamMapper.ToDto(request)));

            return teamId != null
                ? new TeamIdResponse { Id = teamId.ToString() }
                : throw new RpcException(new Status(StatusCode.Internal, "Failed to create team"));
        }
    }
}
