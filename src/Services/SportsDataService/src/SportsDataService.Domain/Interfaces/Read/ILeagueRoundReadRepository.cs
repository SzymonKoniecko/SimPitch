using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ILeagueRoundReadRepository
{
    Task<IEnumerable<LeagueRound>> GetLeagueRoundsBySeasonYearAsync(string seasonYear, CancellationToken cancellationToken);
}
