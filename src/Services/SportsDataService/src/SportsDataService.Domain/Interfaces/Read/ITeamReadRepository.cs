using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ITeamReadRepository
{
    Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken);
    Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken cancellationToken);
}