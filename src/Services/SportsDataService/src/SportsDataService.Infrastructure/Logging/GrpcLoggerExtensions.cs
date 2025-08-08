using LoggingService.SimPitchProtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SportsDataService.Infrastructure.Logging;

public static class GrpcLoggerExtensions
{
    public static ILoggingBuilder AddGrpcLogger(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var section = configuration.GetSection(GrpcLoggerOptions.SectionName);

        // OPTION B: bind bez użycia Microsoft.Extensions.Options.ConfigurationExtensions
        builder.Services.Configure<GrpcLoggerOptions>(options => section.Bind(options));

        // Dodaj klienta gRPC
        builder.Services.AddGrpcClient<LogService.LogServiceClient>((services, options) =>
        {
            var config = services.GetRequiredService<IConfiguration>();
            // wymaga Microsoft.Extensions.Configuration.Binder (GetValue / Bind)
            var address = config.GetValue<string>($"{GrpcLoggerOptions.SectionName}:Address");

            if (string.IsNullOrEmpty(address))
            {
                throw new InvalidOperationException("gRPC LoggingService address is not configured.");
            }

            options.Address = new Uri(address);
        });

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, GrpcLoggerProvider>());

        return builder;
    }
}
