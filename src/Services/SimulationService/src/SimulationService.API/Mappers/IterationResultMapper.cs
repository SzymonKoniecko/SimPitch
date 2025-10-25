using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.API.Mappers;

public static class IterationResultMapper
{
    public static IterationResultGrpc ToProto(IterationResultDto dto)
    {
        return new IterationResultGrpc
        {
            Id = dto.Id.ToString(),
            SimulationId = dto.SimulationId.ToString(),
            IterationIndex = dto.IterationIndex,
            StartDate = dto.StartDate.ToString("o"),
            ExecutionTime = dto.ExecutionTime.ToString(),
            TeamStrengths = JsonConvert.SerializeObject(dto.TeamStrengths),
            SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds),
            LeagueStrength = dto.LeagueStrength,
            PriorLeagueStrength = dto.PriorLeagueStrength,
        };
    }
}
