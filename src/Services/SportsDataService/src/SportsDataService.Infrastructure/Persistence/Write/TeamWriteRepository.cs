using System;
using Dapper;
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
    public async Task CreateTeamAsync(Team team, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO SportsDataDb.dbo.Team
                (Id, Name, CountryId, StadiumId, LeagueId, LogoUrl, ShortName)
                VALUES(@Id, @Name, @CountryId, @StadiumId, @LeagueId, @LogoUrl, @ShortName);
        ";

        if (team.Id == Guid.Empty)
            team.Id = Guid.NewGuid();

        var command = new CommandDefinition(
            commandText: sql,
            parameters: new
            {
                team.Id,
                team.Name,
                team.CountryId,
                team.StadiumId,
                team.LeagueId,
                team.LogoUrl,
                team.ShortName
            },
            cancellationToken: cancellationToken
        );

        await connection.QueryFirstOrDefaultAsync<string>(command);

        return;
    }

    public async Task UpdateTeamAsync(Team team, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("UpdateTeamAsync method is not implemented yet.");
    }

    public async Task DeleteTeamAsync(int teamId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DeleteTeamAsync method is not implemented yet.");
    }
}
