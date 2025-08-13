using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;

public record GetSeasonStatsByIdQuery(Guid seasonStatsId) : IRequest<SeasonStatsDto>;