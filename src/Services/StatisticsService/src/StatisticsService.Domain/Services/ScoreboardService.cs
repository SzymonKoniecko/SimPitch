using System;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Domain.Services;

public class ScoreboardService
{
    private readonly ScoreboardTeamStatsService _scoreboardTeamStatsService;

    public ScoreboardService(ScoreboardTeamStatsService scoreboardTeamStatsService)
    {
        this._scoreboardTeamStatsService = scoreboardTeamStatsService;
    }

    public Scoreboard CalculateSingleScoreboard(IterationResult IterationResult, List<MatchRound> playedMatchRounds)
    {
        Scoreboard scoreboard = new Scoreboard(
            Guid.NewGuid(),
            IterationResult.SimulationId,
            IterationResult.Id,
            IterationResult.LeagueStrength,
            IterationResult.PriorLeagueStrength,
            DateTime.Now
        );
        List<MatchRound> matches = playedMatchRounds
            .Concat(IterationResult.SimulatedMatchRounds)
            .DistinctBy(x => x.Id)
            .ToList();
        scoreboard.AddTeamRange(
            _scoreboardTeamStatsService.CalculateScoreboardTeamStats(
                scoreboard.Id,
                matches
            )
        );
        
        return scoreboard;
    }
}
