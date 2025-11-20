using System;
using Newtonsoft.Json;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class SimulationOverviewMapper
{
    public static SimulationOverviewDto ToDto(SimulationOverview domain)
    {
        var dto = new SimulationOverviewDto();
        dto.Id = domain.Id;
        dto.CreatedDate = domain.CreatedDate;
        dto.SimulationParams = JsonConvert.DeserializeObject<SimulationParamsDto>(domain.SimulationParams);
        dto.LeagueStrengths = JsonConvert.DeserializeObject<List<LeagueStrengthDto>>(domain.LeagueStrengthsJSON);
        dto.PriorLeagueStrength = domain.PriorLeagueStrength;

        foreach (var leagueStrength in dto.LeagueStrengths)
        {
            leagueStrength.SeasonYear = EnumMapper.StringedIntToSeasonString(leagueStrength.SeasonYear);
        }
        
        return dto;
    }
}
