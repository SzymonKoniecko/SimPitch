using System;
using Microsoft.IdentityModel.Tokens;

namespace EngineService.API;
public static class ConfigHelper
{
    public static string GetSimulationAddress()
    {
        string simulationAddress = Environment.GetEnvironmentVariable("SimulationService__Address");

        if (string.IsNullOrEmpty(simulationAddress)) {
            if (!File.Exists("/.dockerenv"))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                    .Build();

                simulationAddress = config["SimulationService:Address"];
            }
            else
                throw new SystemException("SimulationService address is not declared!");
        }
        return simulationAddress;
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
}
