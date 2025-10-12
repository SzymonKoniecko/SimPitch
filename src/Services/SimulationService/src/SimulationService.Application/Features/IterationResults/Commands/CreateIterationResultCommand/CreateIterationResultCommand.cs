using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.IterationResults.Commands.CreateIterationResultCommand;

public record CreateIterationResultCommand(IterationResultDto IterationResult) : IRequest<bool>;
