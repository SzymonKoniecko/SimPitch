using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Mappers;

public static class ScoreboardMapper
{
    public static Scoreboard ToDomain(ScoreboardDto dto)
    {
        // var scoreboard = new Scoreboard(dto.Id, dto.SimulationId, dto.LeagueStrength, dto.PriorLeagueStrength);
        // foreach (var teamDto in dto.ScoreboardTeams)
        // {
        //     var team = new ScoreboardTeam(teamDto.TeamId, teamDto.Points, teamDto.GoalDifference, teamDto.GoalsScored);
        //     scoreboard.AddTeam(team);
        // }
        // return scoreboard;
        throw new NotImplementedException();
    }
}
