using System;
using Microsoft.IdentityModel.Tokens;

namespace SimulationService.API.Helpers;
public static class ConfigHelper
{
    public static string GetSportsDataAddress()
    {
        string sportsDataAddress = Environment.GetEnvironmentVariable("SportsDataService__Address");

        if (string.IsNullOrEmpty(sportsDataAddress)) {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                sportsDataAddress = config["GrpcSportsDataService:Address"];
            }
            else
                throw new SystemException("SportsDataService address is not declared!");
        }
        return sportsDataAddress;
    }

    public static string GetStatisticsAddress()
    {
        string statisticsAddress = Environment.GetEnvironmentVariable("StatisticsService__Address");

        if (string.IsNullOrEmpty(statisticsAddress))
        {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                statisticsAddress = config["StatisticsService:Address"];
            }
            else
                throw new SystemException("StatisticsService address is not declared!");
        }
        return statisticsAddress;
    }

    public static string GetSimPitchMlAddress()
    {
        string statisticsAddress = Environment.GetEnvironmentVariable("SimPitchMl__Address");

        if (string.IsNullOrEmpty(statisticsAddress))
        {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                statisticsAddress = config["SimPitchMl:Address"];
            }
            else
                throw new SystemException("SimPitchMl address is not declared!");
        }
        return statisticsAddress;
    }
    
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
