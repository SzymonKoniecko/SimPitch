using System;
using Newtonsoft.Json;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class StrengthItemDto
{
    [JsonProperty("offensive")]
    public float Offensive { get; set; }

    [JsonProperty("defensive")]
    public float Defensive { get; set; }
}