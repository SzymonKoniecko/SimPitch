using System;

namespace SportsDataService.Application.DTOs;

public class LeagueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sport { get; set; } = string.Empty;
    public Guid CountryId { get; set; } = Guid.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
