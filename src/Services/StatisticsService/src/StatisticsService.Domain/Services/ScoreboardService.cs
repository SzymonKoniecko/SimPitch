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

    public Scoreboard CalculateSingleScoreboard(
        SimulationParams simulationParams,
        IterationResult IterationResult, 
        List<MatchRound> playedMatchRounds)
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
        scoreboard.AddTeamRangeInitialStats(
            _scoreboardTeamStatsService.CalculateScoreboardTeamStatsForSeasonStats(
                scoreboard.Id, 
                IterationResult.TeamStrengths
                    .Where(x => x.SeasonStats.Id == simulationParams.LeagueRoundId || x.SeasonStats.Id == simulationParams.LeagueId)
                    .Select(x => x.SeasonStats)
                    .ToList()
        ));

        return scoreboard;
    }

    public Scoreboard CalculateScoreboardForSeasonStats(List<SeasonStats> seasonStats)
    {
        Scoreboard scoreboard = new(Guid.NewGuid(), Guid.Empty, Guid.Empty, DateTime.Now);
        scoreboard.AddTeamRange(
            _scoreboardTeamStatsService.CalculateScoreboardTeamStatsForSeasonStats(
                scoreboard.Id,
                seasonStats
            )
        );
        scoreboard.SetRankings();

        return scoreboard;
    }
    
    public Scoreboard CalculateScoreboardForPlayedMatchRounds(List<MatchRound> playedMatchRounds)
    {
        Scoreboard scoreboard = new(Guid.NewGuid(), Guid.Empty, Guid.Empty, DateTime.Now);
        scoreboard.AddTeamRange(
            _scoreboardTeamStatsService.CalculateScoreboardTeamStats(
                scoreboard.Id,
                playedMatchRounds
            )
        );
        scoreboard.SetRankings();

        return scoreboard;
    }
}
