using System;
using SimulationService.Application.Features.MatchRounds.DTOs;

namespace SimulationService.Application.Features.IterationResults.DTOs;

public class IterationResultDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int IterationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<TeamStrengthDto> TeamStrengths { get; set; }
    public List<MatchRoundDto> SimulatedMatchRounds { get; set; }
}
