using System;
using System.Data;
using Dapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Domain.Entities;

namespace SportsDataService.Tests.Consts;

public class GrpcTestFixture : IAsyncLifetime
{
    public GrpcChannel GrpcChannel { get; private set; } = default!;

    private TestServer _server = default!;
    private IServiceProvider _services = default!;
    private bool _schemaEnsured;

    public async Task InitializeAsync()
    {
        _server = new TestServer(new WebHostBuilder()
            .UseStartup<TestStartup>());

        GrpcChannel = GrpcChannel.ForAddress(_server.BaseAddress, new GrpcChannelOptions
        {
            HttpHandler = _server.CreateHandler()
        });

        await Task.CompletedTask;
    }

    public async Task InsertTestCountryAsync(Country country)
    {
        await EnsureSchemaAsync();

        var conn = _services.GetRequiredService<IDbConnection>();

        const string sql = @"INSERT OR REPLACE INTO Country (Id, [Name], [Code]) 
                                VALUES (@Id, 'Poland', 'PL');";
        await conn.ExecuteAsync(sql, new { country.Id, country.Name, country.Code});
    }

    /// <summary>
    /// Idempotentne utworzenie schematu tabeli na potrzeby testów.
    /// </summary>
    private async Task EnsureSchemaAsync()
    {
        if (_schemaEnsured) return;

        var conn = _services.GetRequiredService<IDbConnection>();
        const string createSql = @"CREATE TABLE IF NOT EXISTS Country(
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [Code] NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        UpdatedAt DATETIME2 NOT NULL);";

        await conn.ExecuteAsync(createSql);
        _schemaEnsured = true;
    }

    public Task DisposeAsync()
    {
        _server.Dispose();
        return Task.CompletedTask;
    }
}