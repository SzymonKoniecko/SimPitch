using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Mappers;

public static class SimulationMapper
{
    public static SimulationDto ToSimulationDto(
        Guid simulationId,
        int completedIterations,
        SimulationParamsDto simulationParamsDto,
        List<IterationPreviewDto> iterationPreviewList,
        int simulatedMatches,
        float priorLeagueStrength)
    {
        var dto = new SimulationDto();
        dto.Id = simulationId;
        dto.CompletedIterations = completedIterations;
        dto.SimulationParams = simulationParamsDto;
        dto.IterationPreviews = iterationPreviewList;
        dto.SimulatedMatches = simulatedMatches;
        dto.PriorLeagueStrength = priorLeagueStrength;

        return dto;
    }

    internal static SimulationDto ToSimulationDto(Guid simulationId, int count1, SimulationParamsDto simulationParams, List<IterationPreviewDto> iterationPreviewDtos, int? count2, float priorLeagueStrength)
    {
        throw new NotImplementedException();
    }
}
