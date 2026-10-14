using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EngineService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;

public class GetSimulationStatsBySimulationIdQueryHandler : IRequestHandler<GetSimulationStatsBySimulationIdQuery, List<SimulationTeamStatsDto>>
{
    private readonly ISimulationStatsGrpcClient _simulationStatsGrpcClient;
    private readonly ILogger<GetSimulationStatsBySimulationIdQueryHandler> _logger;

    public GetSimulationStatsBySimulationIdQueryHandler(
        ISimulationStatsGrpcClient simulationStatsGrpcClient,
        ILogger<GetSimulationStatsBySimulationIdQueryHandler> logger)
    {
        _simulationStatsGrpcClient = simulationStatsGrpcClient;
        _logger = logger;
    }

    public async Task<List<SimulationTeamStatsDto>> Handle(GetSimulationStatsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var response = await _simulationStatsGrpcClient.CreateSimulationTeamStatsAsync(query.SimulationId, cancellationToken);

        if (response.Item1 == false)
        {
            _logger.LogWarning("SimulationTeamStats is not created for simulation {SimulationId} (no scoreboards yet?)", query.SimulationId);
        }

        return await _simulationStatsGrpcClient.GetSimulationStatsBySimulationIdAsync(query.SimulationId, cancellationToken);
    }
}
