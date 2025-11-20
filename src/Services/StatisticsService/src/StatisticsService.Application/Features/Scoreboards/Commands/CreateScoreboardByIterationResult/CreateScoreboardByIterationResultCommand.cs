using System;
using MediatR;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Domain.ValueObjects;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;

public record CreateScoreboardByIterationResultCommand(SimulationOverview Overview, IterationResultDto iterationResultDto) : IRequest<bool>;