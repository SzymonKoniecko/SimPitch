using System;
using StatisticsService.Application.DTOs.Clients;

namespace StatisticsService.Application.DTOs;

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
