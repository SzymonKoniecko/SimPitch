using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.League.Queries.GetLeagueById;

public record GetLeagueByIdQuery(Guid LeagueId) : IRequest<LeagueDto>;
