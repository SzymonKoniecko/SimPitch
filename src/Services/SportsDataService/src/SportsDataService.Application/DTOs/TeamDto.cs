using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.DTOs;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CountryDto Country { get; set; } = new();
    public StadiumDto Stadium { get; set; } = new();
    public List<CompetitionMembershipDto> Memberships { get; set; } = new();
    public string ShortName { get; set; } = string.Empty;
}
