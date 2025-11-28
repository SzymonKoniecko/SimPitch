using System;

namespace SportsDataService.Domain.Entities;
/// <summary>
/// Represents the real/upcoming match resuls of rounds in given league
/// Used to predict power of team
/// </summary>
public class MatchRound
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public bool? IsDraw { get; set; }
    public bool IsPlayed { get; set; }
}