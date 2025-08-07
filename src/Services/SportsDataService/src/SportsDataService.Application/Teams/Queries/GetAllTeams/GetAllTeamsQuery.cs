using MediatR;
using SportsDataService.Domain.Entities;

public record GetAllTeamsQuery : IRequest<IEnumerable<Team>>;