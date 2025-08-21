using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.LeagueRound.DTOs;

namespace SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

public record GetAllLeagueRoundsByParamsQuery(LeagueRoundFilterDto leagueRoundFilterDto) : IRequest<IEnumerable<LeagueRoundDto>>;