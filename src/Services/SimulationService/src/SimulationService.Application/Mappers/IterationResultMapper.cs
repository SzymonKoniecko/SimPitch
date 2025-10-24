using Newtonsoft.Json;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.IterationResults.DTOs;
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
        Dictionary<Guid, List<TeamStrength>> teamStrengthsDictionary)
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

        dto.TeamStrengths = new();
        foreach (var (key, value) in teamStrengthsDictionary)
        {
            dto.TeamStrengths.AddRange(value.Select(x => TeamStrengthToDto(x)));
        }

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
        entity.TeamStrengths = JsonConvert.SerializeObject(dto.TeamStrengths);
        entity.SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds);
        entity.LeagueStrength = dto.LeagueStrength;
        entity.PriorLeagueStrength = dto.PriorLeagueStrength;

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
        dto.TeamStrengths = !string.IsNullOrWhiteSpace(entity.TeamStrengths)
            ? JsonConvert.DeserializeObject<List<TeamStrengthDto>>(entity.TeamStrengths)
            ?? new List<TeamStrengthDto>()
            : new List<TeamStrengthDto>();
        dto.SimulatedMatchRounds = !string.IsNullOrWhiteSpace(entity.SimulatedMatchRounds)
            ? JsonConvert.DeserializeObject<List<MatchRoundDto>>(entity.SimulatedMatchRounds)
            ?? new List<MatchRoundDto>()
            : new List<MatchRoundDto>();
        dto.LeagueStrength = entity.LeagueStrength;
        dto.PriorLeagueStrength = entity.PriorLeagueStrength;

        return dto;
    }

    public static IEnumerable<IterationResultDto> ToDtoBulk(IEnumerable<IterationResult> entities)
    {
        return entities.Select(ToDto).ToList();
    }

    public static TeamStrengthDto TeamStrengthToDto(TeamStrength teamStrength)
    {
        var dto = new TeamStrengthDto();

        dto.TeamId = teamStrength.TeamId;
        dto.Likelihood = new();
        dto.Likelihood.Offensive = teamStrength.Likelihood.Offensive;
        dto.Likelihood.Defensive = teamStrength.Likelihood.Defensive;
        dto.Posterior = new();
        dto.Posterior.Offensive = teamStrength.Posterior.Offensive;
        dto.Posterior.Defensive = teamStrength.Posterior.Defensive;
        dto.ExpectedGoals = teamStrength.ExpectedGoals;
        dto.LastUpdate = teamStrength.LastUpdate;
        dto.RoundId = teamStrength.RoundId;
        dto.SeasonStats = SeasonStatsMapper.VoToDto(teamStrength.SeasonStats);

        return dto;
    }
}
