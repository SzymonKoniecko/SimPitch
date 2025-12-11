using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Mappers;

public static class ScoreboardMapper
{
    public static Scoreboard ToDomain(ScoreboardDto dto)
    {
        throw new NotImplementedException();
    }

    public static ScoreboardDto ToDto(Scoreboard domain)
    {
        var dto = new ScoreboardDto
        {
            Id = domain.Id,
            SimulationId = domain.SimulationId,
            IterationResultId = domain.IterationResultId,
            ScoreboardTeams = domain.ScoreboardTeams.Select(team => ScoreboardTeamStatsMapper.ToDto(team)).ToList(),
            InitialScoreboardTeams = domain.ScoreboardTeams.Select(team => ScoreboardTeamStatsMapper.ToDto(team)).ToList(),
            CreatedAt = domain.CreatedAt
        };
        
        return dto;
    }
}
