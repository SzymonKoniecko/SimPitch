using System;
using System.Globalization;
using Grpc.Core;
using Newtonsoft.Json;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.Scoreboard;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Entities;

namespace SimulationService.Infrastructure.Clients;

public class ScoreboardGrpcClient : IScoreboardGrpcClient
{
    private readonly ScoreboardService.ScoreboardServiceClient _client;

    public ScoreboardGrpcClient(ScoreboardService.ScoreboardServiceClient scoreboardServiceClient)
    {
        _client = scoreboardServiceClient ?? throw new ArgumentNullException(nameof(scoreboardServiceClient));
    }

    public async Task<bool> CreateScoreboardByIterationResultDataAsync(SimulationOverviewDto Overview, IterationResultDto iterationResultDto, CancellationToken cancellationToken)
    {
        var request = new CreateScoreboardByIterationResultDataRequest
        {
            SimulationOverviewJson = JsonConvert.SerializeObject(Overview),
            IterationResultJson = JsonConvert.SerializeObject(iterationResultDto)
        };

        var response = await _client.CreateScoreboardByIterationResultDataAsync(request, cancellationToken: cancellationToken);

        return response.IsCreated;
    }
}
