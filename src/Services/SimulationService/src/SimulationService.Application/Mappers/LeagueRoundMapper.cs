using System;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class LeagueRoundMapper
{
    public static LeagueRound ToDomain(LeagueRoundDto dto)
    {
        var entity = new LeagueRound();
        entity.Id = dto.Id;
        entity.LeagueId = dto.LeagueId;
        entity.SeasonYear = dto.SeasonYear;
        entity.Round = dto.Round;
        entity.MaxRound = dto.MaxRound;

        return entity;
    }
}
