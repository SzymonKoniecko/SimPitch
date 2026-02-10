using System;
using SimulationService.Application.Features.Predict.DTOs;

namespace SimulationService.Application.Interfaces;

public interface IPredictGrpcClient
{
    Task<PredictResponseDto> StartPredictionAsync(PredictRequestDto predictRequest, CancellationToken cancellationToken);
}
