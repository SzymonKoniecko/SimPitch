using System;

namespace SportsDataService.Application.DTOs;

public class LeagueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; } = Guid.Empty;
    public int MaxRound { get; set; }
    public List<LeagueStrengthDto> Strengths { get; set; }
}
