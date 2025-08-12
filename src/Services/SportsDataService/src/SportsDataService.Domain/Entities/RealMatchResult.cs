using System;

namespace SportsDataService.Domain.Entities;
/// <summary>
/// Represents the real match resuls of rounds in given league
/// Used to predict power of team
/// </summary>
public class RealMatchResult
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public bool IsDraw { get; set; }
}