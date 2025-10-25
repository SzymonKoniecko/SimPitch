using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;

public record RunSimulationCommand(Guid simulationId, SimulationParamsDto SimulationParamsDto, SimulationState State) : IRequest<Guid>;