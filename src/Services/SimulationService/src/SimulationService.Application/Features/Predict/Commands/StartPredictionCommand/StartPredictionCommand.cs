using System;
using MediatR;
using SimulationService.Application.Features.Predict.DTOs;

namespace SimulationService.Application.Features.Predict.Commands.StartPredictionCommand;

public record StartPredictionCommand(PredictRequestDto PredictRequest) : IRequest<PredictResponseDto>;