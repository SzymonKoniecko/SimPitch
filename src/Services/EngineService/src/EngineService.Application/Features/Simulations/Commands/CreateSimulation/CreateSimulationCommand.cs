using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Commands.CreateSimulation;

public record CreateSimulationCommand(SimulationParamsDto simulationParamsDto) : IRequest<string>;