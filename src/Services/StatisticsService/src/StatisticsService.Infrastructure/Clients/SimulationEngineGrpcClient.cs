using System;
using System.Globalization;
using Google.Protobuf.WellKnownTypes;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Application.Interfaces;
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

    public async Task<(List<SimulationOverview>, int)> GetPagedSimulationOverviewsAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var request = new PagedRequest();
        request.Offset = offset;
        request.Limit = limit;
        var response = await _client.GetAllSimulationOverviewsAsync(request, cancellationToken: cancellationToken);

        return (
            response.Items.Select(x => ToDto(x)).ToList(),
            response.TotalCount);
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
            // log dla czytelności
            Console.WriteLine($"➡️ Pobieranie strony {pageNumber} (offset={offset}, limit={pageSize})");

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
        dto.Title = grpc.Title;
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

        dto.SeasonYears = grpc.SeasonYears.ToList();
        dto.Iterations = grpc.Iterations;
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.LeagueRoundId = grpc.HasLeagueRoundId ? Guid.Parse(grpc.LeagueRoundId) : Guid.Empty;

        return dto;
    }
}
