using Grpc.Core;
using Newtonsoft.Json;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.IterationResult;
using StatisticsService.Application.Common.Pagination;
using StatisticsService.Application.Consts;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Infrastructure.Clients;

public class IterationResultGrpcClient : IIterationResultGrpcClient
{
    private readonly IterationResultService.IterationResultServiceClient _client;
    public IterationResultGrpcClient(IterationResultService.IterationResultServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<List<IterationResultDto>> GetPagedIterationResultsBySimulationIdAsync(
        Guid simulationId,
        PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var offset = (pagedRequest.PageNumber - 1) * pagedRequest.PageSize;

        var request = new IterationResultsBySimulationIdRequest
        {
            SimulationId = simulationId.ToString(),
            PagedRequest = new PagedRequestGrpc
            {
                Offset = offset,
                Limit = pagedRequest.PageSize
            }
        };

        var results = new List<IterationResultDto>();

        using var call = _client.GetIterationResultsBySimulationId(request, cancellationToken: cancellationToken);

        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            if (response?.Items != null)
            {
                var mapped = ResponseMapToDto(response);
                results.AddRange(mapped);
            }
        }

        return results;
    }

    public async Task<List<IterationResultDto>> GetAllIterationResultsBySimulationIdAsync(
        Guid simulationId,
        int pageSize = Pagination.PAGINATION_PAGE_LIMIT,
        CancellationToken cancellationToken = default)
    {
        var allResults = new List<IterationResultDto>();
        int pageNumber = 0;

        while (true)
        {
            pageNumber++;

            var page = await GetPagedIterationResultsBySimulationIdAsync(
                simulationId,
                new PagedRequestDto(pageNumber, pageSize),
                cancellationToken);

            if (page == null || page.Count == 0)
                break; // no more data to fetch

            allResults.AddRange(page);

            
            if (page.Count < pageSize) // its the last iteration
                break;

        }

        return allResults;
    }

    private static List<IterationResultDto> ResponseMapToDto(IterationResultsBySimulationIdResponse response)
    {
        List<IterationResultDto> dtos = new List<IterationResultDto>();
        foreach (var result in response.Items)
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
}
