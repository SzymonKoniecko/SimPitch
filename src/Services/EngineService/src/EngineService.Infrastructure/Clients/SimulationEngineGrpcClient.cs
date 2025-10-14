using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
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
