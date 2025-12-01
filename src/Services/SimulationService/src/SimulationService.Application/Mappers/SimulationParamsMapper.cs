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
        vo.LeagueRoundId = dto.LeagueRoundId;
        vo.TargetLeagueRoundId = dto.TargetLeagueRoundId;
        vo.Iterations = dto.Iterations;
        vo.Seed = dto.Seed;
        vo.CreateScoreboardOnCompleteIteration = dto.CreateScoreboardOnCompleteIteration;
        vo.GamesToReachTrust = dto.GamesToReachTrust;
        vo.ConfidenceLevel = dto.ConfidenceLevel;
        vo.HomeAdvantage = dto.HomeAdvantage;
        vo.NoiseFactor = dto.NoiseFactor;
        vo.ModelType = dto.ModelType.ToString();

        return vo;
    }

    public static SimulationParamsDto ToDto(SimulationParams vo)
    {
        var dto = new SimulationParamsDto();

        dto.Title = vo.Title;
        dto.SeasonYears = vo.SeasonYears;
        dto.LeagueId = vo.LeagueId;
        dto.LeagueRoundId = vo.LeagueRoundId;
        dto.TargetLeagueRoundId = vo.TargetLeagueRoundId;
        dto.Iterations = vo.Iterations;
        dto.Seed = vo.Seed;
        dto.CreateScoreboardOnCompleteIteration = vo.CreateScoreboardOnCompleteIteration;
        dto.GamesToReachTrust = vo.GamesToReachTrust;
        dto.ConfidenceLevel = vo.ConfidenceLevel;
        dto.HomeAdvantage = vo.HomeAdvantage;
        dto.NoiseFactor = vo.NoiseFactor;
        dto.ModelType = EnumMapper.StringtoModelTypeEnum(vo.ModelType);

        return dto;
    }
}
