using System;
using MediatR;
using SimulationService.Application.Features.Predict.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Application.Features.Predict.Commands;

public class StartPredictionCommandHandler : IRequestHandler<StartPredictionCommand, PredictResponseDto>
{
    private readonly IPredictGrpcClient _predictGrpcClient;

    public StartPredictionCommandHandler(IPredictGrpcClient predictGrpcClient)
    {
        this._predictGrpcClient = predictGrpcClient;
    }

    public async Task<PredictResponseDto> Handle(StartPredictionCommand command, CancellationToken cancellationToken)
    {
        var response = await _predictGrpcClient.StartPredictionAsync(command.PredictRequest, cancellationToken);

        if (response.Status != "Completed")
        {
            throw new Exception($"Prediction failed, returned status {response.Status} for simulationId: {command.PredictRequest.SimulationId}");
        }
        
        return response;
    }
}
