using System;
using Newtonsoft.Json;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Mappers;

public static class SimulationOverviewMapper
{
    public static SimulationOverview ToDomain(SimulationOverviewDto dto)
    {
        var domain = new SimulationOverview();
        domain.Id = dto.Id;
        domain.Title = dto.Title;
        domain.CreatedDate = dto.CreatedDate;
        domain.SimulationParams = SimulationParamsMapper.ToValueObject(dto.SimulationParams);

        return domain;
    }

    public static SimulationOverviewDto ToDto(SimulationOverview domain)
    {
        var dto = new SimulationOverviewDto();
        dto.Id = domain.Id;
        dto.Title = domain.Title;
        dto.CreatedDate = domain.CreatedDate;
        dto.SimulationParams = SimulationParamsMapper.ToDto(domain.SimulationParams);

        return dto;
    }
}
