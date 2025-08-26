using System;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationEngineMapper
{
    public static SimulationParamsDto SimulationEngineReqestToDto(RunSimulationEngineRequest request)
    {
        var dto = new SimulationParamsDto();
        dto.SeasonYears = request.SeasonYears.ToList();
        dto.LeagueId = Guid.Parse(request.LeagueId);
        dto.Iterations = request.Iterations;
        dto.LeagueRoundId =  request.HasRoundId ? Guid.Parse(request.RoundId) : Guid.Empty;
        
        return dto;
    }
}
