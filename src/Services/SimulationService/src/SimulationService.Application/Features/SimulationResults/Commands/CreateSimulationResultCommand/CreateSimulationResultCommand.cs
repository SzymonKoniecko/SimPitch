using System;
using MediatR;
using SimulationService.Application.Features.SimulationResults.DTOs;

namespace SimulationService.Application.Features.SimulationResults.Commands.CreateSimulationResultCommand;

public record CreateSimulationResultCommand(SimulationResultDto SimulationResult) : IRequest<bool>;
