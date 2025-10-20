using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using Google.Protobuf.WellKnownTypes;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;

namespace EngineService.Infrastructure.Clients;

public class SimulationEngineGrpcClient : ISimulationEngineGrpcClient
{
    private readonly SimulationEngineService.SimulationEngineServiceClient _client;

    public SimulationEngineGrpcClient(SimulationEngineService.SimulationEngineServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<string> CreateSimulation(SimulationParamsDto simulationParamsDto, CancellationToken cancellationToken)
    {
        var request = new RunSimulationEngineRequest();
        request.SimulationParams = ToProto(
            simulationParamsDto.SeasonYears,
            simulationParamsDto.LeagueId,
            simulationParamsDto.Iterations,
            simulationParamsDto.LeagueRoundId);

        var response = await _client.RunSimulationAsync(request, cancellationToken: cancellationToken);

        return response.SimulationId;
    }

    public async Task<List<SimulationOverviewDto>> GetSimulationOverviewsAsync(CancellationToken cancellationToken)
    {
        var request = new Empty();
        var response = await _client.GetAllSimulationOverviewsAsync(request, cancellationToken: cancellationToken);

        return response.SimulationOverviews.Select(x => ToDto(x)).ToList();
    }

    private static SimulationOverviewDto ToDto(SimulationOverviewGrpc grpc)
    {
        var dto = new SimulationOverviewDto();

        dto.Id = Guid.Parse(grpc.Id);
        dto.Title = grpc.Title;
        dto.CreatedDate = DateTime.Parse(grpc.CreatedDate);
        dto.SimulationParams = SimulationParamsToDto(grpc.SimulationParams);

        return dto;
    }

    private static SimulationParamsDto SimulationParamsToDto(SimulationParamsGrpc grpc)
    {
        var dto = new SimulationParamsDto();

        dto.SeasonYears = grpc.SeasonYears.ToList();
        dto.Iterations = grpc.Iterations;
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.LeagueRoundId = grpc.HasLeagueRoundId ? Guid.Parse(grpc.LeagueRoundId) : Guid.Empty;

        return dto;
    }

    private static SimulationParamsGrpc ToProto(List<string> seasonYears, Guid leagueId, int iterations, Guid? leagueRoundId = default)
    {
        var grpc = new SimulationParamsGrpc
        {
            LeagueId = leagueId.ToString(),
            Iterations = iterations
        };

        grpc.SeasonYears.AddRange(seasonYears);

        if (leagueRoundId.HasValue)
            grpc.LeagueRoundId = leagueRoundId.ToString();

        return grpc;
    }
}
