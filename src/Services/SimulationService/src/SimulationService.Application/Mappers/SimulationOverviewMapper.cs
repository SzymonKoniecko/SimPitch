using System;
using Newtonsoft.Json;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Mappers;

public static class SimulationOverviewMapper
{
    public static SimulationOverview ToDomain(SimulationOverviewDto dto)
    {
        var domain = new SimulationOverview();
        domain.Id = dto.Id;
        domain.Title = dto.Title;
        domain.CreatedDate = dto.CreatedDate;
        domain.SimulationParams = JsonConvert.SerializeObject(dto.SimulationParams);

        return domain;
    }

    public static SimulationOverviewDto ToDto(SimulationOverview domain)
    {
        var dto = new SimulationOverviewDto();
        dto.Id = domain.Id;
        dto.Title = domain.Title;
        dto.CreatedDate = domain.CreatedDate;
        dto.SimulationParams = JsonConvert.DeserializeObject<SimulationParamsDto>(domain.SimulationParams);

        return dto;
    }
}
