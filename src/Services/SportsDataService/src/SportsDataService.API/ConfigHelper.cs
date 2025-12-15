using System;
using Microsoft.IdentityModel.Tokens;

namespace SportsDataService.API;
public static class ConfigHelper
{
    internal static string GetLoggerSourceName()
    {
        string serviceName = Environment.GetEnvironmentVariable("GrpcLogging__SourceName");

        if (string.IsNullOrEmpty(serviceName))
        {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                serviceName = config["GrpcLogging:SourceName"];
            }
            else
                throw new SystemException("Service name is not declared!");
        }
        return serviceName;
    }

    internal static string GetRedisCacheConnectionString()
    {
        string serviceName = Environment.GetEnvironmentVariable("ConnectionStrings__Redis");

        if (string.IsNullOrEmpty(serviceName))
        {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                serviceName = config["ConnectionStrings:Redis"];
            }
            else
                throw new SystemException("ConnectionString for REDIS name is not declared!");
        }
        
        return serviceName;
    }
}