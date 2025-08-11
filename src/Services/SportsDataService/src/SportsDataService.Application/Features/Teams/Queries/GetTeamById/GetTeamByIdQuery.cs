using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.Teams.Queries.GetTeamById;
public record GetTeamByIdQuery(Guid TeamId) : IRequest<TeamDto>;
