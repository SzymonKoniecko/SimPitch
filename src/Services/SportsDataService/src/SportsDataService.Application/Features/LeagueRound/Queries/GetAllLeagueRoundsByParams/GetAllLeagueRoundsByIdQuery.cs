using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

public record GetAllLeagueRoundsByParamsQuery(string seasonYear, int? round, Guid? leagueRoundId) : IRequest<IEnumerable<LeagueRoundDto>>;