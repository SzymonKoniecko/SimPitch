using System;
using System.Globalization;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using StatisticsService.Application.Interfaces;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Infrastructure.Clients;

public class SimulationEngineGrpcClient : ISimulationEngineGrpcClient
{
    private readonly SimulationEngineService.SimulationEngineServiceClient _client;

    public SimulationEngineGrpcClient(SimulationEngineService.SimulationEngineServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<SimulationOverview> GetSimulationOverviewByIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new GetSimulationOverviewByIdRequest();
        request.SimulationId = simulationId.ToString();

        var response = await _client.GetSimulationOverviewByIdAsync(request, cancellationToken: cancellationToken);

        return ToDto(response.SimulationOverview);
    }

    public async Task<SimulationState> GetSimulationStateByIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new GetSimulationStateByIdRequest();
        request.SimulationId = simulationId.ToString();
        
        var response = await _client.GetSimulationStateByIdAsync(request, cancellationToken: cancellationToken);

        return GrpcStateToDto(response.SimulationState);
    }

    private async Task<(List<SimulationOverview>, int)> GetPagedSimulationOverviewsAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var request = new PagedRequestGrpc();
        request.Offset = offset;
        request.Limit = limit;
        var response = await _client.GetAllSimulationOverviewsAsync(request, cancellationToken: cancellationToken);

        return (
            response.Items.Select(x => ToDto(x)).ToList(),
            response.Paged.TotalCount);
    }

    public async Task<List<SimulationOverview>> GetAllSimulationOverviewsAsync(
        int pageSize = 100,
        CancellationToken cancellationToken = default)
    {
        var allItems = new List<SimulationOverview>();
        int offset = 0;
        int totalFetched = 0;
        int pageNumber = 1;

        while (true)
        {
            var result = await GetPagedSimulationOverviewsAsync(offset, pageSize, cancellationToken);
            var page = result.Item1;
            if (page == null || page.Count == 0)
            {
                break;
            }

            allItems.AddRange(page);
            totalFetched += page.Count;


            if (page.Count < pageSize) // last page -> end
            {
                break;
            }

            offset += pageSize;
            pageNumber++;
        }

        return allItems;
    }

    private static SimulationOverview ToDto(SimulationOverviewGrpc grpc)
    {
        var dto = new SimulationOverview();

        dto.Id = Guid.Parse(grpc.Id);
        dto.CreatedDate = DateTime.ParseExact(
            grpc.CreatedDate,
            "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture);
        dto.SimulationParams = SimulationParamsToValueObject(grpc.SimulationParams);

        return dto;
    }

    private static SimulationParams SimulationParamsToValueObject(SimulationParamsGrpc grpc)
    {
        var dto = new SimulationParams();

        dto.Title = grpc.Title;
        dto.SeasonYears = grpc.SeasonYears.ToList();
        dto.Iterations = grpc.Iterations;
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.LeagueRoundId = grpc.HasLeagueRoundId ? Guid.Parse(grpc.LeagueRoundId) : Guid.Empty;
        dto.TargetLeagueRoundId = grpc.HasTargetLeagueRoundId ? Guid.Parse(grpc.TargetLeagueRoundId) : Guid.Empty;

        return dto;
    }

    private SimulationState GrpcStateToDto(SimulationStateGrpc simulationState)
    {
        var vo = new SimulationState();
        vo.Id = Guid.Parse(simulationState.Id);
        vo.SimulationId = Guid.Parse(simulationState.SimulationId);
        vo.LastCompletedIteration = simulationState.LastCompletedIteration;
        vo.ProgressPercent = simulationState.ProgressPercent;
        vo.State = simulationState.State;
        vo.UpdatedAt = DateTime.ParseExact(
            simulationState.UpdatedAt,
            "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture
        );

        return vo;
    }
}
