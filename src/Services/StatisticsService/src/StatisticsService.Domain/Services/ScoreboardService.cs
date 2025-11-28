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
            DateTime.Now
        );
        HashSet<Guid> simulatedIds = IterationResult.SimulatedMatchRounds
            .Select(x => x.Id)
            .ToHashSet();

        List<MatchRound> matches = playedMatchRounds
            .Where(playedMD => !simulatedIds.Contains(playedMD.Id))
            .Concat(IterationResult.SimulatedMatchRounds)
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
