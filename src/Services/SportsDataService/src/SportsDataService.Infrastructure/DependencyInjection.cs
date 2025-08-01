using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsDataService.Application.Interfaces;
using SportsDataService.Infrastructure.Persistence;

namespace SportsDataService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        return services;
    }
}
