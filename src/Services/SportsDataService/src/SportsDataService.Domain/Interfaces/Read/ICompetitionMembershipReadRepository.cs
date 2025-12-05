using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ICompetitionMembershipReadRepository
{
    Task<IEnumerable<CompetitionMembership>> GetAllByLeagueIdAndSeasonYearAsync(Guid leagueId, string seasonYear, CancellationToken cancellationToken);
    Task<IEnumerable<CompetitionMembership>> GetAllByTeamIdAsync(Guid teamId, CancellationToken cancellationToken);
}
