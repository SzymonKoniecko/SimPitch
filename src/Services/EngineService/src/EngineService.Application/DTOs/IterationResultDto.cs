using System;

namespace EngineService.Application.DTOs;

public class IterationResultDto
{
    public Guid Id { get; set; }
    public Guid SimulationId { get; set; }
    public int IterationIndex { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public List<TeamStrengthDto> TeamStrengths { get; set; }
    public List<MatchRoundDto> SimulatedMatchRounds { get; set; }
    public float LeagueStrength { get; set; }
    public float PriorLeagueStrength { get; set; }
}
