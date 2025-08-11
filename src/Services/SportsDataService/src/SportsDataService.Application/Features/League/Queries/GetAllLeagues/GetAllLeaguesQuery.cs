using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.League.Queries.GetAllLeagues;

public record GetAllLeaguesQuery : IRequest<IEnumerable<LeagueDto>>;