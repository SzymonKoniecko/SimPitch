using System;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.SimulationResults.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class SimulationResultMapper
{
    public static SimulationResultDto SimulationToDto(
        Guid simulationId,
        int simulationIndex,
        DateTime simulationDate,
        TimeSpan executionTime,
        List<MatchRound> simulatedMatchRounds,
        float leagueStrength,
        float priorLeagueStrength,
        string raport)
    {
        var dto = new SimulationResultDto();
        dto.Id = Guid.NewGuid();
        dto.SimulationId = simulationId;
        dto.SimulationIndex = simulationIndex;
        dto.StartDate = simulationDate;
        dto.ExecutionTime = executionTime;
        dto.SimulatedMatchRounds = (List<MatchRoundDto>)MatchRoundMapper.ToDtoBulk(simulatedMatchRounds);
        dto.LeagueStrength = leagueStrength;
        dto.PriorLeagueStrength = priorLeagueStrength;
        dto.Raport = raport;

        return dto;
    }

    public static SimulationResult ToDomain(SimulationResultDto dto)
    {
        var entity = new SimulationResult();
        entity.Id = dto.Id;
        entity.SimulationId = dto.SimulationId;
        entity.SimulationIndex = dto.SimulationIndex;
        entity.StartDate = dto.StartDate;
        entity.ExecutionTime = dto.ExecutionTime;
        entity.SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds);
        entity.LeagueStrength = dto.LeagueStrength;
        entity.PriorLeagueStrength = dto.PriorLeagueStrength;
        entity.Raport = dto.Raport;

        return entity;
    }

    public static IEnumerable<SimulationResult> ToDomainBulk(IEnumerable<SimulationResultDto> dtos)
    {
        return dtos.Select(ToDomain).ToList();
    }

    public static SimulationResultDto ToDto(SimulationResult entity)
    {
        var dto = new SimulationResultDto();
        dto.Id = entity.Id;
        dto.SimulationId = entity.SimulationId;
        dto.SimulationIndex = entity.SimulationIndex;
        dto.StartDate = entity.StartDate;
        dto.ExecutionTime = entity.ExecutionTime;
        dto.SimulatedMatchRounds = (List<MatchRoundDto>)MatchRoundMapper.ToDtoBulk(entity.SimulatedMatchRounds != null
            ? JsonConvert.DeserializeObject<List<MatchRound>>(entity.SimulatedMatchRounds)
            : new List<MatchRound>());
        dto.LeagueStrength = entity.LeagueStrength;
        dto.PriorLeagueStrength = entity.PriorLeagueStrength;
        dto.Raport = entity.Raport;

        return dto;
    }

    public static IEnumerable<SimulationResultDto> ToDtoBulk(IEnumerable<SimulationResult> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}
