using System;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Interfaces;

public interface IScoreboardGrpcClient
{
    Task<bool> CreateScoreboardByIterationResultDataAsync(SimulationOverviewDto Overview, IterationResultDto iterationResultDto, CancellationToken cancellationToken);
}
