using System;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.SimulationResults.DTOs;

public class SimulationResultDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int SimulationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<MatchRoundDto> SimulatedMatchRounds { get; set; }
    public float LeagueStrength { get; set; }
    public float PriorLeagueStrength { get; set; }
    public SimulationParamsDto SimulationParams { get; set; }
    public string Raport { get; set; }
}
