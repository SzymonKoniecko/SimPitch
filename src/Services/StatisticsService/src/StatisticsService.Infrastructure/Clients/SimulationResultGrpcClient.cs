using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationResult;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Infrastructure.Clients;

public class SimulationResultGrpcClient : ISimulationResultGrpcClient
{
    private readonly SimulationResultService.SimulationResultServiceClient _client;
    public SimulationResultGrpcClient(SimulationResultService.SimulationResultServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<List<SimulationResultDto>> GetSimulationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new SimulationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString()
        };

        var response = await _client.GetSimulationResultsBySimulationIdAsync(request, cancellationToken: cancellationToken);

        return MapToDto(response);
    }

    private static List<SimulationResultDto> MapToDto(SimulationResultsBySimulationIdResponse response)
    {
        List<SimulationResultDto> dtos = new List<SimulationResultDto>();
        foreach (var result in response.SimulationResults)
        {
            var dto = new SimulationResultDto();

            dto.Id = Guid.Parse(result.Id);
            dto.SimulationId = Guid.Parse(result.SimulationId);
            dto.SimulationIndex = result.SimulationIndex;
            dto.StartDate = DateTime.Parse(result.StartDate);
            dto.ExecutionTime = TimeSpan.Parse(result.ExecutionTime);
            dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(result.SimulatedMatchRounds);
            dto.LeagueStrength = result.LeagueStrength;
            dto.PriorLeagueStrength = result.PriorLeagueStrength;
            dto.SimulationParams = MapProtoToDto(result.SimulationParams);
            dto.Raport = result.Raport;

            dtos.Add(dto);
        }

        return dtos;
    }

    private static SimulationParamsDto MapProtoToDto(SimulationParamsGrpc proto)
    {
        if (proto == null)
            return null;

        var dto = new SimulationParamsDto();

        dto.SeasonYears = proto.SeasonYears.ToList();
        dto.LeagueId = Guid.Parse(proto.LeagueId);
        dto.Iterations = proto.Iterations;
        dto.LeagueRoundId = proto.HasLeagueRoundId ? Guid.Parse(proto.LeagueRoundId) : Guid.Empty;
        
        return dto;
    }
}
