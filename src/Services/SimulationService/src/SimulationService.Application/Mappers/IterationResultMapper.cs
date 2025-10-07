using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Mappers;

public static class IterationResultMapper
{
    public static IterationResultDto SimulationToDto(
        Guid simulationId,
        int iterationIndex,
        DateTime simulationDate,
        TimeSpan executionTime,
        List<MatchRound> simulatedMatchRounds,
        float leagueStrength,
        float priorLeagueStrength,
        SimulationParams simulationParams,
        string raport)
    {
        var dto = new IterationResultDto();
        dto.Id = Guid.NewGuid();
        dto.SimulationId = simulationId;
        dto.IterationIndex = iterationIndex;
        dto.StartDate = simulationDate;
        dto.ExecutionTime = executionTime;
        dto.SimulatedMatchRounds = (List<MatchRoundDto>)MatchRoundMapper.ToDtoBulk(simulatedMatchRounds);
        dto.LeagueStrength = leagueStrength;
        dto.PriorLeagueStrength = priorLeagueStrength;
        dto.SimulationParams = new SimulationParamsDto
        {
            SeasonYears = simulationParams.SeasonYears,
            LeagueId = simulationParams.LeagueId,
            Iterations = simulationParams.Iterations,
            LeagueRoundId = simulationParams.LeagueRoundId
        };
        dto.Raport = raport;

        return dto;
    }

    public static IterationResult ToDomain(IterationResultDto dto)
    {
        var entity = new IterationResult();
        entity.Id = dto.Id;
        entity.SimulationId = dto.SimulationId;
        entity.IterationIndex = dto.IterationIndex;
        entity.StartDate = dto.StartDate;
        entity.ExecutionTime = dto.ExecutionTime;
        entity.SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds);
        entity.LeagueStrength = dto.LeagueStrength;
        entity.PriorLeagueStrength = dto.PriorLeagueStrength;
        entity.SimulationParams = dto.SimulationParams != null ? JsonConvert.SerializeObject(dto.SimulationParams) : null;
        entity.Raport = dto.Raport;

        return entity;
    }

    public static IEnumerable<IterationResult> ToDomainBulk(IEnumerable<IterationResultDto> dtos)
    {
        return dtos.Select(ToDomain).ToList();
    }

    public static IterationResultDto ToDto(IterationResult entity)
    {
        var dto = new IterationResultDto();
        dto.Id = entity.Id;
        dto.SimulationId = entity.SimulationId;
        dto.IterationIndex = entity.IterationIndex;
        dto.StartDate = entity.StartDate;
        dto.ExecutionTime = entity.ExecutionTime;
        dto.SimulatedMatchRounds = (List<MatchRoundDto>)MatchRoundMapper.ToDtoBulk(entity.SimulatedMatchRounds != null
            ? JsonConvert.DeserializeObject<List<MatchRound>>(entity.SimulatedMatchRounds)
            : new List<MatchRound>());
        dto.LeagueStrength = entity.LeagueStrength;
        dto.PriorLeagueStrength = entity.PriorLeagueStrength;
        dto.SimulationParams = entity.SimulationParams != null
            ? JsonConvert.DeserializeObject<SimulationParamsDto>(entity.SimulationParams)
            : null;
        dto.Raport = entity.Raport;

        return dto;
    }

    public static IEnumerable<IterationResultDto> ToDtoBulk(IEnumerable<IterationResult> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}
