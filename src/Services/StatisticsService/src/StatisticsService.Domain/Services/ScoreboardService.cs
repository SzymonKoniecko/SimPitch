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

    public Scoreboard CalculateSingleScoreboard(SimulationResult simulationResult, List<MatchRound> playedMatchRounds)
    {
        Scoreboard scoreboard = new Scoreboard(
            Guid.NewGuid(),
            simulationResult.SimulationId,
            simulationResult.Id,
            simulationResult.LeagueStrength,
            simulationResult.PriorLeagueStrength
        );
        List<MatchRound> matches = playedMatchRounds.Where(x => simulationResult.SimulatedMatchRounds.Any(smr => smr.Id != x.Id)).ToList();
        scoreboard.AddTeamRange(
            _scoreboardTeamStatsService.CalculateScoreboardTeamStats(
                scoreboard.Id,
                matches
            )
        );
        
        return scoreboard;
    }
}
