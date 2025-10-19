using System;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class IterationResultDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int IterationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<MatchRoundDto> SimulatedMatchRounds { get; set; }
    public float LeagueStrength { get; set; }
    public float PriorLeagueStrength { get; set; }
    public string Raport { get; set; }
}
