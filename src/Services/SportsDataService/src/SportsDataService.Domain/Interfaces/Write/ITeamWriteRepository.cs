using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Write;

public interface ITeamWriteRepository
{
    Task<Guid> CreateTeamAsync(Team team, CancellationToken cancellationToken);
    Task UpdateTeamAsync(Team team, CancellationToken cancellationToken);
    Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken);
}
