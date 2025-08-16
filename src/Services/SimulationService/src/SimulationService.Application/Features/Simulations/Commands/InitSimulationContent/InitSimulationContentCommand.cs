using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;

public record InitSimulationContentCommand(SimulationParamsDto SimulationParamsDto) : IRequest<List<MatchRound>>;