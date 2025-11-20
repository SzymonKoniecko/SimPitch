using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ILeagueStrengthReadRepository
{
    Task<List<LeagueStrength>> GetLeagueStrengthsByLeagueIdAsync(Guid leagueId, CancellationToken cancellationToken);
}
