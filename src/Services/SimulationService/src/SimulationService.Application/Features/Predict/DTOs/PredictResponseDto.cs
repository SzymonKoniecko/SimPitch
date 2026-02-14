using System;

namespace SimulationService.Application.Features.Predict.DTOs;

public class PredictResponseDto
{
    public string Status { get; set; }
    public int IterationCount { get; set; }
}
