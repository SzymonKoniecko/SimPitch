using System;
using MediatR;
using StatisticsService.Application.DTOs;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByLeagueIdAndSeasonYear;

public record CreateScoreboardByLeagueIdAndSeasonYearCommand(Guid leagueId, string seasonYear) : IRequest<ScoreboardDto>;