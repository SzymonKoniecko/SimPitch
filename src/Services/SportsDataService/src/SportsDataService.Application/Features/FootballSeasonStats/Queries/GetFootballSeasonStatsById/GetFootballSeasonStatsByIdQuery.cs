using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.FootballSeasonStats.Queries.GetFootballSeasonStatsById;

public record GetFootballSeasonStatsByIdQuery(Guid FootballSeasonStatsId) : IRequest<FootballSeasonStatsDto>;