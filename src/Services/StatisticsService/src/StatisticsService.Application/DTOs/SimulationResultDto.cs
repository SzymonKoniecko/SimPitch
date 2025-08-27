using System;

namespace StatisticsService.Application.DTOs;

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
    public string Raport { get; set; }
}
