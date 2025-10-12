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
            SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds),
            LeagueStrength = dto.LeagueStrength,
            PriorLeagueStrength = dto.PriorLeagueStrength,
            SimulationParams = SimulationParamsToProto(dto.SimulationParams),
            Raport = dto.Raport
        };
    }
    
    private static SimulationParamsGrpc SimulationParamsToProto(SimulationParamsDto dto)
    {
        if (dto == null)
        {
            return null;
        }
        
        var proto = new SimulationParamsGrpc
        {
            SeasonYears = { dto.SeasonYears },
            LeagueId = dto.LeagueId.ToString(),
            Iterations = dto.Iterations
        };

        if (dto.LeagueRoundId != Guid.Empty)
        {
            proto.LeagueRoundId = dto.LeagueRoundId.ToString();
        }

        return proto;
    }
}
