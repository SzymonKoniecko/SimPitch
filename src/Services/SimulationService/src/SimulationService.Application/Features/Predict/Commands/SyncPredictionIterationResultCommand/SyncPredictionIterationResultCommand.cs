using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.Predict.Commands.SyncPredictionIterationResultCommand;

public record SyncPredictionIterationResultCommand(IterationResultDto IterationResult) : IRequest<bool>;
