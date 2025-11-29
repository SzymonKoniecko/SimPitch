using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Mappers;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.Services;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByLeagueIdAndSeasonYear;

public class CreateScoreboardByLeagueIdAndSeasonYearCommandHandler : IRequestHandler<CreateScoreboardByLeagueIdAndSeasonYearCommand, ScoreboardDto>
{
    private readonly ISeasonStatsGrpcClient _seasonStatsGrpcClient;
    private readonly ScoreboardService _scoreboardService;

    public CreateScoreboardByLeagueIdAndSeasonYearCommandHandler(
        ISeasonStatsGrpcClient seasonStatsGrpcClient,
        ScoreboardService scoreboardService)
    {
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
        _scoreboardService = scoreboardService;
    }
    
    public async Task<ScoreboardDto> Handle(CreateScoreboardByLeagueIdAndSeasonYearCommand Command, CancellationToken cancellationToken)
    {
        var seasonStats = await _seasonStatsGrpcClient.GetSeasonStatsByLeagueAndSeasonYear(Command.leagueId, Command.seasonYear, cancellationToken);

        if (seasonStats == null || seasonStats.Count() == 0)
        {
            return null;
        }
        
        var scoreboard = _scoreboardService.CalculateScoreboardForSeasonStats(seasonStats);

        return ScoreboardMapper.ToDto(scoreboard);
    }
}
