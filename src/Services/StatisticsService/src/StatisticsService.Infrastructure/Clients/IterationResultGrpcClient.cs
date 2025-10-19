using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.IterationResult;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Infrastructure.Clients;

public class IterationResultGrpcClient : IIterationResultGrpcClient
{
    private readonly IterationResultService.IterationResultServiceClient _client;
    public IterationResultGrpcClient(IterationResultService.IterationResultServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<List<IterationResultDto>> GetIterationResultsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new IterationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString()
        };

        var response = await _client.GetIterationResultsBySimulationIdAsync(request, cancellationToken: cancellationToken);

        return MapToDto(response);
    }

    private static List<IterationResultDto> MapToDto(IterationResultsBySimulationIdResponse response)
    {
        List<IterationResultDto> dtos = new List<IterationResultDto>();
        foreach (var result in response.IterationResults)
        {
            var dto = new IterationResultDto();

            dto.Id = Guid.Parse(result.Id);
            dto.SimulationId = Guid.Parse(result.SimulationId);
            dto.IterationIndex = result.IterationIndex;
            dto.StartDate = DateTime.Parse(result.StartDate);
            dto.ExecutionTime = TimeSpan.Parse(result.ExecutionTime);
            dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(result.SimulatedMatchRounds);
            dto.LeagueStrength = result.LeagueStrength;
            dto.PriorLeagueStrength = result.PriorLeagueStrength;
            dto.Raport = result.Raport;

            dtos.Add(dto);
        }

        return dtos;
    }
}
