using System;
using MediatR;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Scoreboards.Commands;

public record CreateScoreboardByIterationResultCommand(SimulationOverviewDto OverviewDto, IterationResultDto IterationResultDto) : IRequest<bool>;