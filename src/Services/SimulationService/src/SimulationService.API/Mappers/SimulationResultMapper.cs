using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimulationService.Application.Features.SimulationResults.DTOs;

namespace SimulationService.API.Mappers;

public static class SimulationResultMapper
{
    public static SimulationResultGrpc ToProto(SimulationResultDto dto)
    {
        return new SimulationResultGrpc
        {
            Id = dto.Id.ToString(),
            SimulationId = dto.SimulationId.ToString(),
            SimulationIndex = dto.SimulationIndex,
            StartDate = dto.StartDate.ToString("o"),
            ExecutionTime = dto.ExecutionTime.ToString(),
            SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds),
            LeagueStrength = dto.LeagueStrength,
            PriorLeagueStrength = dto.PriorLeagueStrength,
            Raport = dto.Raport
        };
    }
}
