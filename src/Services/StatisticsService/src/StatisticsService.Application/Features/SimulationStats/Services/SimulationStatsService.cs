using System;
using Microsoft.Extensions.Logging;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Features.SimulationStats.Services;

public class SimulationStatsService : ISimulationStatsService
{
    private readonly ILogger<SimulationStatsService> _logger;

    public SimulationStatsService(ILogger<SimulationStatsService> logger)
    {
        _logger = logger;
    }
    public List<SimulationTeamStats> CalculateSimulationStatsForTeams(List<ScoreboardTeamStats> scoreboardTeamStats, Guid simulationId)
    {
        List<SimulationTeamStats> simulationTeamStats = new();
        int positionCount = scoreboardTeamStats.Max(x => x.Rank);

        foreach (var teamId in scoreboardTeamStats.Select(x => x.TeamId).Distinct())
        {
            simulationTeamStats.Add(
                CalculateSimulationStatsForSingleTeam(
                    scoreboardTeamStats.Where(x => x.TeamId == teamId).ToList(),
                    simulationId,
                    teamId,
                    positionCount
                )
            );
        }
        if (positionCount != simulationTeamStats.Count)
            throw new Exception("Calculated position number is not equal to simulationTeamStats collection!");

        return simulationTeamStats;
    }

    public SimulationTeamStats CalculateSimulationStatsForSingleTeam(List<ScoreboardTeamStats> teamScoreboardStats, Guid simulationId, Guid teamId, int positionCount)
    {
        if (teamScoreboardStats == null || teamScoreboardStats.Count == 0)
        {
            _logger.LogWarning($"Missing scoreboard stats for single team: {teamId}");
            return new SimulationTeamStats(simulationId: simulationId, teamId, positionCount);
        }
        SimulationTeamStats simulationTeamStats = new(simulationId: simulationId, teamScoreboardStats.First().TeamId, positionCount);

        foreach (var singleTeamStats in teamScoreboardStats)
        {
            simulationTeamStats.AddFromScoreboardStats(
                singleTeamStats.Rank - 1, // -1 because of array cell
                singleTeamStats.Points,
                singleTeamStats.Wins,
                singleTeamStats.Losses,
                singleTeamStats.Draws,
                singleTeamStats.GoalsFor,
                singleTeamStats.GoalsAgainst
            );
        }
        simulationTeamStats.SetNormalizedPositionProbability();
        return simulationTeamStats;
    }
}
