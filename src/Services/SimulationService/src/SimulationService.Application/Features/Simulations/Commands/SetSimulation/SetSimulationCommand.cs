using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;

namespace SimulationService.Application.Features.Simulations.Commands.SetSimulation;

public record SetSimulationCommand(SimulationParamsDto SimulationParamsDto) : IRequest<Guid>;