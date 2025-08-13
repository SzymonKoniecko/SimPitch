using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Teams.DTOs;
namespace SportsDataService.Application.Features.Teams.Commands.CreateTeam;
public record CreateTeamCommand(CreateTeamDto Team) : IRequest<Guid>;