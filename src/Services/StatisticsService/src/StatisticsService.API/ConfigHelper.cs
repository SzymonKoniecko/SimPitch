using System;
using Microsoft.IdentityModel.Tokens;

namespace StatisticsService.API;

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

    internal static string GetSimulationServiceAddress()
    {
        string simulationServiceAddress = Environment.GetEnvironmentVariable("SimulationService__Address");

        if (string.IsNullOrEmpty(simulationServiceAddress))
        {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                simulationServiceAddress = config["GrpcSimulationService:Address"];
            }
            else
                throw new SystemException("SimulationService address is not declared!");
        }
        return simulationServiceAddress;
    }
    
    public static string GetSportsDataServiceAddress()
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
}
