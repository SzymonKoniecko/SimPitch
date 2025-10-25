using System;
using MediatR;

namespace SimulationService.Application.Features.Simulations.Commands.StopSimulation;

public record StopSimulationCommand(Guid SimulationId) : IRequest<string>;