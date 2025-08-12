using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Domain.Entities;

public class LeagueRound
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
    public int Round { get; set; }
    public int MaxRound { get; set; }
}