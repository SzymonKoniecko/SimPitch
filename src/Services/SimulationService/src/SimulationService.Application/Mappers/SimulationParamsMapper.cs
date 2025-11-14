using System;
using System.Xml;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

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
        vo.CreateScoreboardOnCompleteIteration = dto.CreateScoreboardOnCompleteIteration;
        vo.Seed = dto.Seed;
        vo.GamesToReachTrust = dto.GamesToReachTrust;
        vo.ConfidenceLevel = dto.ConfidenceLevel;
        vo.HomeAdvantage = dto.HomeAdvantage;
        vo.NoiseFactor = dto.NoiseFactor;

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
        dto.CreateScoreboardOnCompleteIteration = vo.CreateScoreboardOnCompleteIteration;
        dto.Seed = vo.Seed;
        dto.GamesToReachTrust = vo.GamesToReachTrust;
        dto.ConfidenceLevel = vo.ConfidenceLevel;
        dto.HomeAdvantage = vo.HomeAdvantage;
        dto.NoiseFactor = vo.NoiseFactor;

        return dto;
    }
}
