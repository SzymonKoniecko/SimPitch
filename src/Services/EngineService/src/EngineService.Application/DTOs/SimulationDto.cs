using System;

namespace EngineService.Application.DTOs;

public class SimulationDto
{
    public Guid Id { get; set; }
    public string WinnersSummary { get; set; } = "1st JAG (30%) 2nd LEG (20%) 3rd ZAG (10%)";
    public SimulationStateDto State { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
    public List<IterationPreviewDto> IterationPreviews { get; set; }
    public int SimulatedMatches { get; set; }
    public float PriorLeagueStrength { get; set; }
}