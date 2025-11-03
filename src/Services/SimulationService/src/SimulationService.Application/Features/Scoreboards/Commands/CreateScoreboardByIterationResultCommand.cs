using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Features.Scoreboards.Commands;

public record CreateScoreboardByIterationResultCommand(IterationResultDto IterationResultDto) : IRequest<bool>;