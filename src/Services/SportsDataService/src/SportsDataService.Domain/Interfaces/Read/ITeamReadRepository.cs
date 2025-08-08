using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Read;

public interface ITeamReadRepository
{
    Task<Team?> GetTeamByIdAsync(Guid teamId);
    Task<IEnumerable<Team>> GetAllTeamsAsync();
}