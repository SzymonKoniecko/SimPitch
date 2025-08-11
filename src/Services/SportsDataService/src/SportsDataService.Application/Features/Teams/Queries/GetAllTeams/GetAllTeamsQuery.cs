using MediatR;
using SportsDataService.Application.DTOs;
namespace SportsDataService.Application.Features.Teams.Queries.GetAllTeams;
public record GetAllTeamsQuery : IRequest<IEnumerable<TeamDto>>;