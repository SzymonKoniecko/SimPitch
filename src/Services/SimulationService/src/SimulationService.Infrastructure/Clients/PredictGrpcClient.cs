using System;
using Newtonsoft.Json;
using SimPitchProtos.SimPitchMl;
using SimPitchProtos.SimPitchMl.Predict;
using SimulationService.Application.Features.Predict.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Infrastructure.Clients;

public class PredictGrpcClient : IPredictGrpcClient
{
    private readonly PredictService.PredictServiceClient _predictServiceClient;

    public PredictGrpcClient(PredictService.PredictServiceClient predictServiceClient)
    {
        this._predictServiceClient = predictServiceClient;
    }

    public async Task<PredictResponseDto> StartPredictionAsync(PredictRequestDto predictRequest, CancellationToken cancellationToken)
    {
        var grpcRequest = new PredictRequest();
        grpcRequest.Predict = ToGrpc(predictRequest);

        var response = await _predictServiceClient.StartPredictionAsync(grpcRequest, cancellationToken: cancellationToken);

        return new PredictResponseDto
        {
            Status = response.Status,
            IterationCount = response.PredictedIterations
        };
    }

    private PredictGrpc ToGrpc(PredictRequestDto predictRequest)
    {
        var grpc = new PredictGrpc();

        grpc.SimulationId = predictRequest.SimulationId.ToString();
        grpc.LeagueId = predictRequest.LeagueId.ToString();
        grpc.IterationCount = predictRequest.IterationCount;
        grpc.TeamStrengths = JsonConvert.SerializeObject(predictRequest.TeamStrengths);
        grpc.MatchesToSimulate = JsonConvert.SerializeObject(predictRequest.MatchesToSimulate);
        grpc.TrainUntilRoundNo = predictRequest.TrainUntilRoundNumber;
        grpc.LeagueAvgStrength = predictRequest.LeagueAverangeStrength ?? 1.7f;
        grpc.Seed = predictRequest.Seed ?? 0;
        grpc.TrainRatio = predictRequest.TrainRatio ?? 0.8f;

        return grpc;
    }
}
