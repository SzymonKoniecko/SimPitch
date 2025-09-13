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

    public Scoreboard CalculateSingleScoreboard(SimulationResult simulationResult)
    {
        Scoreboard scoreboard = new Scoreboard(
            Guid.NewGuid(),
            simulationResult.SimulationId,
            simulationResult.Id,
            simulationResult.LeagueStrength,
            simulationResult.PriorLeagueStrength
        );
        List<Guid> teamIds = simulationResult.SimulatedMatchRounds.Select(x => x.HomeTeamId).ToList();
        teamIds.AddRange(simulationResult.SimulatedMatchRounds.Select(x => x.AwayTeamId));
        teamIds.Distinct().ToList();

        scoreboard.AddTeamRange(
            teamIds.Select(teamId =>
                _scoreboardTeamStatsService.CalculateScoreboardTeamStatsForSingleTeam(
                    simulationResult.SimulationId,
                    teamId,
                    simulationResult.SimulatedMatchRounds.Where(
                        x => x.HomeTeamId == teamId || x.AwayTeamId == teamId
                    ).ToList()
                )
            )
        );
        return scoreboard;
    }
}
