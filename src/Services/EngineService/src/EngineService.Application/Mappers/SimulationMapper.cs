using System;
using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;

namespace EngineService.Application.Mappers;

public static class SimulationMapper
{
    public static SimulationDto ToSimulationDto(
        Guid simulationId,
        SimulationStateDto stateDto,
        SimulationParamsDto simulationParamsDto,
        PagedResponse<IterationPreviewDto> iterationPreviewList,
        int simulatedMatches,
        float priorLeagueStrength)
    {
        var dto = new SimulationDto();
        dto.Id = simulationId;
        dto.State = stateDto;
        dto.SimulationParams = simulationParamsDto;
        dto.IterationPreviews = iterationPreviewList;
        dto.SimulatedMatches = simulatedMatches;
        dto.PriorLeagueStrength = priorLeagueStrength;

        return dto;
    }
}
