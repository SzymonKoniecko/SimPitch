using System;
using SimulationService.Application.Features.IterationResults.DTOs;

namespace SimulationService.Application.Interfaces;

public interface IScoreboardGrpcClient
{
    Task<bool> CreateScoreboardByIterationResultDataAsync(IterationResultDto iterationResultDto, CancellationToken cancellationToken);
}
