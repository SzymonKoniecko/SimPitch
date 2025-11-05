using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.SimulationStats;

namespace EngineService.Infrastructure.Clients;

public class SimulationStatsGrpcClient : ISimulationStatsGrpcClient
{
    private readonly SimulationStatsService.SimulationStatsServiceClient _client;

    public SimulationStatsGrpcClient(SimulationStatsService.SimulationStatsServiceClient simulationStatsServiceClient)
    {
        _client = simulationStatsServiceClient ?? throw new ArgumentNullException(nameof(simulationStatsServiceClient));
    }

    /// <summary>
    /// Returns (bool - isCompleted; Guid - simulationId)
    /// </summary>
    public async Task<(bool, Guid)> CreateSimulationTeamStatsAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new CreateSimulationStatsRequest
        {
            SimulationId = simulationId.ToString()
        };

        var response = await _client.CreateSimulationStatsAsync(request, cancellationToken: cancellationToken);

        return (response.IsCreated, Guid.Parse(response.SimulationId));
    }

    public async Task<List<SimulationTeamStatsDto>> GetSimulationStatsBySimulationIdAsync(Guid simulationId, CancellationToken cancellationToken)
    {
        var request = new GetSimulationStatsRequest
        {
            SimulationId = simulationId.ToString()
        };

        var response = await _client.GetSimulationStatsAsync(request, cancellationToken: cancellationToken);

        return response.SimulationTeamStats.Select(x => ToDto(x)).ToList();
    }

    private SimulationTeamStatsDto ToDto(SimulationTeamStatsGrpc grpc)
    {
        var dto = new SimulationTeamStatsDto();


        dto.Id = Guid.Parse(grpc.Id);
        dto.SimulationId = Guid.Parse(grpc.SimulationId);
        dto.TeamId = Guid.Parse(grpc.TeamId);

        dto.PositionProbbility = grpc.PositionProbbility.ToArray();

        dto.AverangePoints = grpc.AverangePoints;
        dto.AverangeWins = grpc.AverangeWins;
        dto.AverangeLosses = grpc.AverangeLosses;
        dto.AverangeDraws = grpc.AverangeDraws;
        dto.AverangeGoalsFor = grpc.AverangeGoalsFor;
        dto.AverangeGoalsAgainst = grpc.AverangeGoalsAgainst;

        return dto;
    }
}
