using System;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationEngineMapper
{
    public static SimulationParamsDto SimulationEngineReqestToDto(RunSimulationEngineRequest request)
    {
        var dto = new SimulationParamsDto();
        dto.SeasonYear = request.SeasonYear;
        dto.RoundId =  request.HasRoundId ? Guid.Parse(request.RoundId) : Guid.Empty;
        dto.LeagueId = request.HasLeagueId ? Guid.Parse(request.LeagueId) : Guid.Empty;
        
        return dto;
    }
}
