
using SportsDataService.Domain.Entities;

namespace SportsDataService.Application.Interfaces;
public interface ITeamRepository
{
    Task<Team?> GetTeamByIdAsync(Guid teamId);
    Task<IEnumerable<Team>> GetAllTeamsAsync();
    Task AddTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task DeleteTeamAsync(int teamId);
}