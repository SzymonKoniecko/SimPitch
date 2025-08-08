using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SportsDataService.Infrastructure.Clients;

namespace SportsDataService.Infrastructure.Logging;
public static class GrpcLoggerExtensions
{
    public static ILoggingBuilder AddGrpcLogger(this ILoggingBuilder builder, string sourceName)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Services.TryAddSingleton<IGrpcLoggingClient, GrpcLoggingClient>();
        builder.Services.AddSingleton<ILoggerProvider>(sp =>
        {
            var client = sp.GetRequiredService<IGrpcLoggingClient>();
            return new GrpcLoggerProvider(client, sourceName);
        });

        return builder;
    }
}