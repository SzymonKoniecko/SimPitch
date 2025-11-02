using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.IterationResult;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using Grpc.Core;
using EngineService.Application.Common.Pagination;
using EngineService.Domain.ValueObjects;
using Google.Protobuf.Collections;
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

    public async Task<(List<IterationResultDto>, PagedResponseDetails)> GetIterationResultsBySimulationIdAsync(Guid simulationId, PagedRequest pagedRequest, CancellationToken cancellationToken)
    {
        var offset = (pagedRequest.PageNumber - 1) * pagedRequest.PageSize;

        var request = new IterationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString(),
            PagedRequest = new PagedRequestGrpc
            {
                Offset = offset,
                Limit = pagedRequest.PageSize,
                SortingMethod = new SortingMethodGrpc
                {
                    SortingOption = pagedRequest.SortingMethod.SortingOption.ToString(),
                    Order = pagedRequest.SortingMethod.Order
                }
            }
        };
        var results = new List<IterationResultDto>();
        PagedResponseDetails details = new();

        using var call = _client.GetIterationResultsBySimulationId(request, cancellationToken: cancellationToken);


        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            if (response?.Items != null)
            {
                var mapped = MapToDto(response.Items);
                results.AddRange(mapped);
                details = MapToDto(response.Paged, pagedRequest);
            }
        }
        return (
            results,
            details
        );
    }

    private PagedResponseDetails MapToDto(PagedResponseGrpc grpc, PagedRequest pagedRequest)
    {
        var details = new PagedResponseDetails();

        details.TotalCount = grpc.TotalCount;
        details.PageNumber = pagedRequest.PageNumber;
        details.PageSize = pagedRequest.PageSize;
        details.SortingOption = grpc.SortingOption;
        details.Order = grpc.SortingOrder;

        return details;
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
