using System;
using EngineService.Application.DTOs;
using MediatR;

namespace EngineService.Application.Features.Simulations.Commands.StopSimulation;

public record StopSimulationCommand(Guid SimulationId) : IRequest<SimulationStateDto>;