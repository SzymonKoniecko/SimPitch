using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.IterationResult;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using Google.Protobuf.Collections;
using Grpc.Core;

namespace EngineService.Infrastructure.Clients;

public class IterationResultGrpcClient : IIterationResultGrpcClient
{
    private readonly IterationResultService.IterationResultServiceClient _client;
    public IterationResultGrpcClient(IterationResultService.IterationResultServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<IterationResultDto> GetIterationResultByIdAsync(Guid iterationId, CancellationToken cancellationToken)
    {
        var request = new IterationResultByIdRequest
        {
            Id = iterationId.ToString()
        };

        var response = await _client.GetIterationResultByIdAsync(request, cancellationToken: cancellationToken);

        return MapToDto(response.IterationResult);
    }

    public async Task<(List<IterationResultDto>, int)> GetIterationResultsBySimulationIdAsync(Guid simulationId, int offset, int limit, CancellationToken cancellationToken)
    {
        var request = new IterationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString(),
            PagedRequest = new PagedRequest
            {
                Offset = offset,
                Limit = limit
            }
        };
        var results = new List<IterationResultDto>();
        int totalCount = 0;

        using var call = _client.GetIterationResultsBySimulationId(request, cancellationToken: cancellationToken);


        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            if (response?.Items != null)
            {
                var mapped = MapToDto(response.Items);
                results.AddRange(mapped);
                totalCount = response.TotalCount;
            }
        }
        return (
            results,
            totalCount
        );
    }
    private List<IterationResultDto> MapToDto(RepeatedField<IterationResultGrpc> iterationResults)
    {
        List<IterationResultDto> dtos = new List<IterationResultDto>();
        foreach (var result in iterationResults)
        {
            var dto = new IterationResultDto();

            dto.Id = Guid.Parse(result.Id);
            dto.SimulationId = Guid.Parse(result.SimulationId);
            dto.IterationIndex = result.IterationIndex;
            dto.StartDate = DateTime.Parse(result.StartDate);
            dto.ExecutionTime = TimeSpan.Parse(result.ExecutionTime);
            dto.TeamStrengths = JsonConvert.DeserializeObject<List<TeamStrengthDto>>(result.TeamStrengths);
            dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(result.SimulatedMatchRounds);
            dto.LeagueStrength = result.LeagueStrength;
            dto.PriorLeagueStrength = result.PriorLeagueStrength;

            dtos.Add(dto);
        }

        return dtos;
    }

    private static IterationResultDto MapToDto(IterationResultGrpc result)
    {
        if (result == null)
            return null;

        var dto = new IterationResultDto();

        dto.Id = Guid.Parse(result.Id);
        dto.SimulationId = Guid.Parse(result.SimulationId);
        dto.IterationIndex = result.IterationIndex;
        dto.StartDate = DateTime.Parse(result.StartDate);
        dto.ExecutionTime = TimeSpan.Parse(result.ExecutionTime);
        dto.TeamStrengths = JsonConvert.DeserializeObject<List<TeamStrengthDto>>(result.TeamStrengths);
        dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(result.SimulatedMatchRounds);
        dto.LeagueStrength = result.LeagueStrength;
        dto.PriorLeagueStrength = result.PriorLeagueStrength;
        
        return dto;
    }
}
