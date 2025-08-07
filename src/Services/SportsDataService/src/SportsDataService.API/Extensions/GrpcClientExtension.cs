

namespace SportsDataService.API.Extensions;
public static class GrpcClientExtension
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddGrpcClient<LoggingService.SimPitchProtos.LogService>(options =>
        // {
        //     var envLoggingServiceUrl = Environment.GetEnvironmentVariable("LOGGING_SERVICE_URL");
        //     if (!string.IsNullOrEmpty(envLoggingServiceUrl))
        //         options.Address = new Uri(envLoggingServiceUrl);
        //     else
        //         throw new InvalidOperationException("Logging service URL is not configured.");
        // });

        return services;
    }
}