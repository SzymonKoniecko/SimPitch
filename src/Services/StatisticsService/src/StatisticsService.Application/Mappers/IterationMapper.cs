using System;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class IterationResultMapper
{
    public static IterationResult ToValueObject(IterationResultDto dto)
    {
        var valueObject = new IterationResult();

        valueObject.Id = dto.Id;
        valueObject.SimulationId = dto.SimulationId;
        valueObject.SimulationIndex = dto.IterationIndex;
        valueObject.StartDate = dto.StartDate;
        valueObject.ExecutionTime = dto.ExecutionTime;
        valueObject.TeamStrengths = dto.TeamStrengths.Select(x => MapToValueObject(x)).ToList();
        valueObject.SimulatedMatchRounds = dto.SimulatedMatchRounds
            .Select(x => MatchRoundMapper.ToValueObject(x))
            .ToList();

        return valueObject;
    }

    private static TeamStrength MapToValueObject(TeamStrengthDto dto)
    {
        var vo = new TeamStrength();

        vo.TeamId = dto.TeamId;
        vo.SeasonStats = new SeasonStats()
        {
            Id = dto.SeasonStats.Id,
            TeamId = dto.SeasonStats.TeamId,
            SeasonYear = dto.SeasonStats.SeasonYear,
            LeagueId = dto.SeasonStats.LeagueId,
            LeagueStrength = dto.SeasonStats.LeagueStrength,
            MatchesPlayed = dto.SeasonStats.MatchesPlayed,
            Wins = dto.SeasonStats.Wins,
            Losses = dto.SeasonStats.Losses,
            Draws = dto.SeasonStats.Draws,
            GoalsFor = dto.SeasonStats.GoalsFor,
            GoalsAgainst = dto.SeasonStats.GoalsAgainst
        };

        return vo;
    }
}
