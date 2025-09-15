using System;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationEngineMapper
{
    public static SimulationParamsDto SimulationEngineReqestToDto(RunSimulationEngineRequest request)
    {
        var dto = new SimulationParamsDto();
        dto.SeasonYears = request.SimulationParams.SeasonYears.ToList();
        dto.LeagueId = Guid.Parse(request.SimulationParams.LeagueId);
        dto.Iterations = request.SimulationParams.Iterations;
        dto.LeagueRoundId =  request.SimulationParams.HasLeagueRoundId ? Guid.Parse(request.SimulationParams.LeagueRoundId) : Guid.Empty;
        
        return dto;
    }
}
