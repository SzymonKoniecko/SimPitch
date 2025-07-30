using Dapper;
using SportsDataService.Application.Interfaces;
using SportsDataService.Domain.Entities;
using System.Data;

namespace SportsDataService.Infrastructure.Persistence;

public class TeamRepository : ITeamRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TeamRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Teams WHERE Id = @Id";
        var team = await connection.QueryFirstOrDefaultAsync<Team>(sql, new { Id = teamId });
        if (team == null)
        {
            throw new KeyNotFoundException($"Team with Id '{teamId}' was not found.");
        }
        return team;
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Teams order by Name";
        return await connection.QueryAsync<Team>(sql);
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