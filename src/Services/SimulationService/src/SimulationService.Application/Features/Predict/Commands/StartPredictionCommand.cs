using System;
using MediatR;
using SimulationService.Application.Features.Predict.DTOs;

namespace SimulationService.Application.Features.Predict.Commands;

public record StartPredictionCommand(PredictRequestDto PredictRequest) : IRequest<PredictResponseDto>;