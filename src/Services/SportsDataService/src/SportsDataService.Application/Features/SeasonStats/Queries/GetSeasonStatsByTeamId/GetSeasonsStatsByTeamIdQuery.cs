using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;

public record GetSeasonsStatsByTeamIdQuery(Guid teamId) : IRequest<IEnumerable<SeasonStatsDto>>;