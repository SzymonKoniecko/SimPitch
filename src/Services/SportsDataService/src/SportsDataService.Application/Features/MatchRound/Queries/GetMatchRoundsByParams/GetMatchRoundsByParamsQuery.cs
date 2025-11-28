using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByParams;

public record GetMatchRoundsByParamsQuery(Guid LeagueId, string seasonYear) : IRequest<List<MatchRoundDto>>;