using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.DTOs.Feature;
namespace SportsDataService.Application.Features.Teams.Commands.CreateTeam;
public record CreateTeamCommand(CreateTeamDto Team) : IRequest<Guid>;