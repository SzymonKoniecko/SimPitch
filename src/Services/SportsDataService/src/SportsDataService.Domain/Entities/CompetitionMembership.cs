using System;

namespace SportsDataService.Domain.Entities;

public class CompetitionMembership
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid LeagueId { get; set; }
    public string SeasonYear { get; set; }
}
