using MediatR;
using SportsDataService.Domain.Entities;

public record GetTeamByIdQuery(Guid TeamId) : IRequest<Team>;
