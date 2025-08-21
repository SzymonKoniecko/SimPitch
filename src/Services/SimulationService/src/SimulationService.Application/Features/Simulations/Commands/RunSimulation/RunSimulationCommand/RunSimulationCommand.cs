using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;

public record RunSimulationCommand(SimulationParamsDto SimulationParamsDto) : IRequest<string>;