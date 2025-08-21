using System;

namespace SportsDataService.Application.DTOs;

public class MatchRoundDto
{
    public Guid Id { get; set; }
    public Guid RoundId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public bool IsDraw { get; set; }
    public bool IsPlayed { get; set; }
}
