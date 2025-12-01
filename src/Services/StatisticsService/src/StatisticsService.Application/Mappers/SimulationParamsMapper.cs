using System;
using System.Xml;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class SimulationParamsMapper
{
    public static SimulationParams ToValueObject(SimulationParamsDto dto)
    {
        var vo = new SimulationParams();

        vo.Title = dto.Title;
        vo.SeasonYears = dto.SeasonYears;
        vo.LeagueId = dto.LeagueId;
        vo.Iterations = dto.Iterations;
        vo.LeagueRoundId = dto.LeagueRoundId;
        vo.TargetLeagueRoundId = dto.TargetLeagueRoundId;
        vo.CreateScoreboardOnCompleteIteration = dto.CreateScoreboardOnCompleteIteration;

        return vo;
    }

    public static SimulationParamsDto ToDto(SimulationParams vo)
    {
        var dto = new SimulationParamsDto();

        dto.Title = vo.Title;
        dto.SeasonYears = vo.SeasonYears;
        dto.LeagueId = vo.LeagueId;
        dto.Iterations = vo.Iterations;
        dto.LeagueRoundId = vo.LeagueRoundId;
        dto.TargetLeagueRoundId = vo.TargetLeagueRoundId;
        dto.CreateScoreboardOnCompleteIteration = vo.CreateScoreboardOnCompleteIteration;

        return dto;
    }
}
