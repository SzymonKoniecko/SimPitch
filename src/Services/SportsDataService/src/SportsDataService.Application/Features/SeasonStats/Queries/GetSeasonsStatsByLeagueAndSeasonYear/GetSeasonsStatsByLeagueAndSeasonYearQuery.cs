using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonsStatsByLeagueAndSeasonYear;

public record GetSeasonsStatsByLeagueAndSeasonYearQuery(Guid leagueId, string seasonYear) : IRequest<List<SeasonStatsDto>>;