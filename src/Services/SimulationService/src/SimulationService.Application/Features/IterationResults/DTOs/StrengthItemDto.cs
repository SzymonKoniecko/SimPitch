using System;
using Newtonsoft.Json;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class StrengthItemDto
{
    public float Offensive { get; set; }
    public float Defensive { get; set; }
}