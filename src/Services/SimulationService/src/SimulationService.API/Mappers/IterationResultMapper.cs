using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
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
            SimulatedMatchRounds = JsonConvert.SerializeObject(dto.SimulatedMatchRounds)
        };
    }

    public static IterationResultDto ToDto(IterationResultGrpc grpc)
    {
        var dto = new IterationResultDto();

        dto.Id = Guid.Parse(grpc.Id);
        dto.SimulationId = Guid.Parse(grpc.SimulationId);
        dto.IterationIndex = grpc.HasIterationIndex ? grpc.IterationIndex : 0; 
        dto.StartDate = DateTime.Parse(grpc.StartDate);
        dto.ExecutionTime = TimeSpan.Parse(grpc.ExecutionTime);
        dto.TeamStrengths = JsonConvert.DeserializeObject<List<TeamStrengthDto>>(grpc.TeamStrengths) ?? throw new NullReferenceException("Missing TeamStrengths for IterationResultGrpc");
        dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(grpc.SimulatedMatchRounds) ?? throw new NullReferenceException("Missing SimulatedMatchRounds for IterationResultGrpc");

        return dto;
    }
}
