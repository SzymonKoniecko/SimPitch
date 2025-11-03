using System;
using System.Globalization;
using Grpc.Core;
using Newtonsoft.Json;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.Scoreboard;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Infrastructure.Clients;

public class ScoreboardGrpcClient : IScoreboardGrpcClient
{
    private readonly ScoreboardService.ScoreboardServiceClient _client;

    public ScoreboardGrpcClient(ScoreboardService.ScoreboardServiceClient scoreboardServiceClient)
    {
        _client = scoreboardServiceClient ?? throw new ArgumentNullException(nameof(scoreboardServiceClient));
    }

    public async Task<bool> CreateScoreboardByIterationResultDataAsync(IterationResultDto iterationResultDto, CancellationToken cancellationToken)
    {
        var request = new CreateScoreboardByIterationResultDataRequest
        {
            IterationResultJson = JsonConvert.SerializeObject(iterationResultDto)
        };

        var response = await _client.CreateScoreboardByIterationResultDataAsync(request, cancellationToken: cancellationToken);

        return response.IsCreated;
    }
}
