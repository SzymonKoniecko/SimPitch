using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Interfaces;

public interface ISimulationStatsService
{
    List<SimulationTeamStats> CalculateSimulationStatsForTeams(List<ScoreboardTeamStats> scoreboardTeamStats, Guid simulationId);
    SimulationTeamStats CalculateSimulationStatsForSingleTeam(List<ScoreboardTeamStats> teamScoreboardStats, Guid simulationId, Guid teamId, int positionCount);
}
