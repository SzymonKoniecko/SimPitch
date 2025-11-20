using System;

namespace SportsDataService.Domain.Entities;

public class LeagueStrength
{
    public Guid Id { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
    public float Strength { get; set; }
}
