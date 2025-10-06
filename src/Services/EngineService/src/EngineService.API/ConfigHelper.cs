using System;
using Microsoft.IdentityModel.Tokens;

namespace EngineService.API;
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
