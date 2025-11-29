using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Scoreboards.Queries.GetScoreboardByLeagueIdAndSeasonYear;

public record GetScoreboardByLeagueIdAndSeasonYearQuery(Guid leagueId, string seasonYear) : IRequest<ScoreboardDto>;
