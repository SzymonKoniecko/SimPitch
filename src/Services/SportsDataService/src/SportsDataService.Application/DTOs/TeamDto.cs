using System;
using SportsDataService.Domain.Enums;

namespace SportsDataService.Application.DTOs;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CountryDto Country { get; set; }
    public StadiumDto Stadium { get; set; }
    public LeagueDto League { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}
