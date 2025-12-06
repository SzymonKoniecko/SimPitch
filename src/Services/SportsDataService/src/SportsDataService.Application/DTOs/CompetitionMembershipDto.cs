using System;

namespace SportsDataService.Application.DTOs;

public class CompetitionMembershipDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
}
