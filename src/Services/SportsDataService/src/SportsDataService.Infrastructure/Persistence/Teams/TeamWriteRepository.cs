using System;
using SportsDataService.Domain.Entities;
using SportsDataService.Domain.Interfaces.Write;

namespace SportsDataService.Infrastructure.Persistence.Teams;

public class TeamWriteRepository : ITeamWriteRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TeamWriteRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task AddTeamAsync(Team team)
    {
        throw new NotImplementedException("AddTeamAsync method is not implemented yet.");
    }

    public async Task UpdateTeamAsync(Team team)
    {
        throw new NotImplementedException("UpdateTeamAsync method is not implemented yet.");
    }

    public async Task DeleteTeamAsync(int teamId)
    {
        throw new NotImplementedException("DeleteTeamAsync method is not implemented yet.");
    }
}
