using Newtonsoft.Json;
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
            dto.Raport = result.Raport;
        }

        return dtos;
    }
}
