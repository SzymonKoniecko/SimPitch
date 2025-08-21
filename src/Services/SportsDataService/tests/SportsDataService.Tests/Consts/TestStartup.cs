using System;
using System.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Tests.Consts;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();

        //services.AddMediatR(typeof(GetUserQueryHandler).Assembly);

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        services.AddSingleton<IDbConnection>(connection);

        //services.AddScoped<ICountryReadRepository, CountryReadRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapGrpcService<UserGrpcService>();
        });
    }
}
