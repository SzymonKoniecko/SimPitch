using System;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;
using SimPitchProtos.SimPitchMl;
using SimPitchProtos.SimPitchMl.Predict;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.Predict.Commands.SyncPredictionIterationResultCommand;
using SimulationService.Application.Features.Predict.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;

namespace SimulationService.Infrastructure.Clients;

public class PredictGrpcClient : IPredictGrpcClient
{
    private readonly PredictService.PredictServiceClient _predictServiceClient;
    private readonly IMediator _mediator;

    public PredictGrpcClient(
        PredictService.PredictServiceClient predictServiceClient, IMediator mediator)
    {
        this._predictServiceClient = predictServiceClient;
        this._mediator = mediator;
    }

    public async Task<PredictResponseDto> StreamPredictionAsync(
    PredictRequestDto predictRequest,
    CancellationToken cancellationToken)
    {
        var grpcRequest = new PredictRequest
        {
            Predict = ToGrpc(predictRequest)
        };

        // File.WriteAllText(
        //     Path.Combine(AppContext.BaseDirectory, "predict.json"),
        //     JsonConvert.SerializeObject(grpcRequest.Predict));

        using var call = _predictServiceClient.StreamPrediction(grpcRequest, cancellationToken: cancellationToken);

        string status = "STARTED";
        int currentCounter = 0;

        try
        {
            await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                if (response is null) continue;

                status = response.Status ?? status;
                currentCounter = response.PredictedIterations; // zawsze aktualizuj

                if (response.IterationResult is not null)
                {
                    var command = new SyncPredictionIterationResultCommand(ToDto(response.IterationResult));
                    await _mediator.Send(command, cancellationToken);
                }

                if (string.Equals(status, "COMPLETED", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            status = "CANCELLED";
        }

        return new PredictResponseDto
        {
            Status = status,
            IterationCount = currentCounter
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

    public static IterationResultDto ToDto(IterationResultGrpc grpc)
    {
        var dto = new IterationResultDto();

        dto.Id = Guid.Parse(grpc.Id);
        dto.SimulationId = Guid.Parse(grpc.SimulationId);
        dto.IterationIndex = grpc.HasIterationIndex ? grpc.IterationIndex : 0;
        dto.StartDate = DateTime.Parse(grpc.StartDate);
        dto.ExecutionTime = TimeSpan.Parse(grpc.ExecutionTime);
        dto.TeamStrengths = JsonConvert.DeserializeObject<List<TeamStrengthDto>>(grpc.TeamStrengths) ?? throw new NullReferenceException("Missing TeamStrengths for IterationResultGrpc");
        dto.SimulatedMatchRounds = JsonConvert.DeserializeObject<List<MatchRoundDto>>(grpc.SimulatedMatchRounds) ?? throw new NullReferenceException("Missing SimulatedMatchRounds for IterationResultGrpc");

        return dto;
    }
}
