using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace SimulationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        // Read repositories

        // Write repositories
        
        return services;
    }
}