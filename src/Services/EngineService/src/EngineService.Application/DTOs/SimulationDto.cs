using System;
using EngineService.Application.Common.Pagination;

namespace EngineService.Application.DTOs;

public class  SimulationDto
{
    public Guid Id { get; set; }
    public string WinnersSummary { get; set; } = "UNKNOWN";
    public SimulationStateDto State { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
    public PagedResponse<IterationPreviewDto> IterationPreviews { get; set; }
    public int SimulatedMatches { get; set; }
    public List<LeagueStrengthDto> LeagueStrengths { get; set; }
    public float PriorLeagueStrength { get; set; }
}