using System;
using MediatR;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Leagues.Query.GetLeagueById;

public record GetLeagueByIdQuery(Guid leagueId) : IRequest<League>;