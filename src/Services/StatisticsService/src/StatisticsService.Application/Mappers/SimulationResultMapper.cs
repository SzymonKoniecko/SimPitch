using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class SimulationResultMapper
{
    public static SimulationResult ToValueObject(SimulationResultDto dto)
    {
        var valueObject = new SimulationResult();

        valueObject.Id = dto.Id;
        valueObject.SimulationId = dto.SimulationId;
        valueObject.SimulationIndex = dto.SimulationIndex;
        valueObject.StartDate = dto.StartDate;
        valueObject.ExecutionTime = dto.ExecutionTime;
        valueObject.SimulatedMatchRounds = dto.SimulatedMatchRounds
            .Select(x => MatchRoundMapper.ToValueObject(x))
            .ToList();
        valueObject.LeagueStrength = dto.LeagueStrength;
        valueObject.PriorLeagueStrength = dto.PriorLeagueStrength;
        valueObject.Raport = dto.Raport;

        return valueObject;
    }
}
