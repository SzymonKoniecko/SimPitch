using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByLeagueIdAndSeasonYear;

public class CreateScoreboardByLeagueIdAndSeasonYearCommandHandler : IRequestHandler<CreateScoreboardByLeagueIdAndSeasonYearCommand, ScoreboardDto>
{
    private readonly ILeagueRoundGrpcClient _leagueRoundGrpcClient;
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;
    private readonly ISeasonStatsGrpcClient _seasonStatsGrpcClient;
    private readonly ScoreboardService _scoreboardService;

    public CreateScoreboardByLeagueIdAndSeasonYearCommandHandler(
        ILeagueRoundGrpcClient leagueRoundGrpcClient,
        IMatchRoundGrpcClient matchRoundGrpcClient,
        ISeasonStatsGrpcClient seasonStatsGrpcClient,
        ScoreboardService scoreboardService)
    {
        _leagueRoundGrpcClient = leagueRoundGrpcClient;
        _matchRoundGrpcClient = matchRoundGrpcClient;
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
        _scoreboardService = scoreboardService;
    }

    public async Task<ScoreboardDto> Handle(CreateScoreboardByLeagueIdAndSeasonYearCommand Command, CancellationToken cancellationToken)
    {
        var seasonStats = await _seasonStatsGrpcClient.GetSeasonStatsByLeagueAndSeasonYear(Command.leagueId, Command.seasonYear, cancellationToken);

        if (seasonStats != null && seasonStats.Count() > 0)
        {
            var scoreboard = _scoreboardService.CalculateScoreboardForSeasonStats(seasonStats);
            return ScoreboardMapper.ToDto(scoreboard);
        }
        else
        {
            var request = new LeagueRounds.DTOs.LeagueRoundDtoRequest();
            request.LeagueId = Command.leagueId;
            request.SeasonYears = new List<string> { Command.seasonYear };
            var leagueRounds = await _leagueRoundGrpcClient.GetAllLeagueRoundsByParams(request, cancellationToken);

            if (leagueRounds == null || leagueRounds.Count() == 0)
            {
                return null;
            }

            List<MatchRoundDto> matchRounds = new();
            foreach (var leagueRound in leagueRounds)
            {
                matchRounds.AddRange(await _matchRoundGrpcClient.GetMatchRoundsByRoundIdAsync(leagueRound.Id, cancellationToken));
            }
            var scoreboard = _scoreboardService.CalculateScoreboardForPlayedMatchRounds(
                matchRounds
                    .Where(x => x.IsPlayed == true)
                    .Select(x => MatchRoundMapper.ToValueObject(x))
                    .ToList()
            );
            return ScoreboardMapper.ToDto(scoreboard);
        }
    }
}
