using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EngineService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //  Database
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
    
        //  Clients DI

        //  Read repositories

        //  Write repositories

        // Services

        
        return services;
    }
}