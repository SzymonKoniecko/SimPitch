using System;
using Google.Protobuf.WellKnownTypes;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Application.Interfaces;

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

    public async Task<List<SimulationOverview>> GetSimulationOverviewAsync(CancellationToken cancellationToken)
    {
        var request = new Empty();
        var response = await _client.GetAllSimulationOverviewsAsync(request, cancellationToken: cancellationToken);

        return response.SimulationOverviews.Select(x => ToDto(x)).ToList();
    }

    private static SimulationOverview ToDto(SimulationOverviewGrpc grpc)
    {
        var dto = new SimulationOverview();

        dto.Id = Guid.Parse(grpc.Id);
        dto.Title = grpc.Title;
        dto.CreatedDate = DateTime.Parse(grpc.CreatedDate);
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
