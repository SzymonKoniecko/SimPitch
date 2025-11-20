using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Domain.Entities;

public class League
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CountryId { get; set; }
    public int MaxRound { get; set; }
    public List<LeagueStrength> Strengths { get; set; }
}
