using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ILeagueRoundReadRepository
{
    Task<List<LeagueRound>> GetLeagueRoundsBySeasonYearAsync(string seasonYear, CancellationToken cancellationToken);
}
