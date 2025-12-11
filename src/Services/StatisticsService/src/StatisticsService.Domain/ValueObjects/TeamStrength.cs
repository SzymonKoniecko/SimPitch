using System;

namespace StatisticsService.Domain.ValueObjects;

public class TeamStrength
{
    public Guid TeamId { get; set; }
    public SeasonStats SeasonStats { get; set; }
}
