using System;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Domain.Interfaces.Write;

public interface ITeamWriteRepository
{

    Task AddTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task DeleteTeamAsync(int teamId);
}
