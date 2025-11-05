using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Mappers;

public static class SimulationTeamStatsMapper
{
    public static SimulationTeamStatsDto ToDto(SimulationTeamStats domain)
    {
        var dto = new SimulationTeamStatsDto();

        dto.Id = domain.Id;
        dto.SimulationId = domain.SimulationId;
        dto.TeamId = domain.TeamId;
        dto.PositionProbbility = domain.PositionProbbility;
        dto.AverangePoints = domain.AverangePoints;
        dto.AverangeWins = domain.AverangeWins;
        dto.AverangeLosses = domain.AverangeLosses;
        dto.AverangeDraws = domain.AverangeDraws;
        dto.AverangeGoalsFor = domain.AverangeGoalsFor;
        dto.AverangeGoalsAgainst = domain.AverangeGoalsAgainst;

        return dto;
    }
}
