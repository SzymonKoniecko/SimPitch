using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Domain.Interfaces.Read;
using SportsDataService.Domain.Interfaces.Write;
using SportsDataService.Infrastructure.Persistence.Teams;

namespace SportsDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddTransient<ITeamReadRepository, TeamReadRepository>();
        services.AddTransient<ITeamWriteRepository, TeamWriteRepository>();
        return services;
    }
}