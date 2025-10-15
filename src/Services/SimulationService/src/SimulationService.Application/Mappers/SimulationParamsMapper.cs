using System;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

public static class SimulationParamsMapper
{
    public static SimulationParams ToValueObject(SimulationParamsDto dto)
    {
        var vo = new SimulationParams();

        vo.SeasonYears = dto.SeasonYears;
        vo.LeagueId = dto.LeagueId;
        vo.Iterations = dto.Iterations;
        vo.LeagueRoundId = dto.LeagueRoundId;

        return vo;
    }

    public static SimulationParamsDto ToDto(SimulationParams vo)
    {
        var dto = new SimulationParamsDto();

        dto.SeasonYears = vo.SeasonYears;
        dto.LeagueId = vo.LeagueId;
        dto.Iterations = vo.Iterations;
        dto.LeagueRoundId = vo.LeagueRoundId;

        return dto;
    }
}
