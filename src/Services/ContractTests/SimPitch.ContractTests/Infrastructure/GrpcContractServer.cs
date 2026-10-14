using System.Globalization;
using System.Runtime.CompilerServices;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace SimPitch.ContractTests.Infrastructure;

/// <summary>
/// Hosts real gRPC service implementations (with the service's real exception interceptor) in memory.
/// The application layer behind them is a mocked <see cref="IMediator"/>, so a test controls exactly
/// which data the server maps onto the wire. Clients talk to it over a real gRPC channel.
/// </summary>
public sealed class GrpcContractServer : IAsyncDisposable
{
    private readonly WebApplication _app;

    public Mock<IMediator> Mediator { get; } = new();
    public GrpcChannel Channel { get; }

    private GrpcContractServer(Action<IServiceCollection> configureServices, Action<WebApplication> mapServices)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        builder.Services.AddSingleton(Mediator.Object);
        configureServices(builder.Services);

        _app = builder.Build();
        mapServices(_app);
        _app.StartAsync().GetAwaiter().GetResult();

        Channel = GrpcChannel.ForAddress("http://contract-server", new GrpcChannelOptions
        {
            HttpHandler = _app.GetTestServer().CreateHandler(),
            MaxReceiveMessageSize = int.MaxValue
        });
    }

    /// <param name="mapServices">Maps the server's gRPC services, e.g. <c>app.MapGrpcService&lt;T&gt;()</c>.</param>
    public static GrpcContractServer Start<TInterceptor>(Action<WebApplication> mapServices)
        where TInterceptor : Interceptor
        => new(services =>
        {
            services.AddScoped<TInterceptor>();
            services.AddGrpc(options => options.Interceptors.Add<TInterceptor>());
        }, mapServices);

    public async ValueTask DisposeAsync()
    {
        Channel.Dispose();
        await _app.StopAsync();
        await _app.DisposeAsync();
    }
}

/// <summary>Test data values chosen to expose typical contract breaks.</summary>
public static class ContractData
{
    /// <summary>Day &gt; 12, so swapped day/month formats cannot parse by accident.</summary>
    public static readonly DateTime Timestamp = new(2026, 10, 14, 13, 45, 30, DateTimeKind.Utc);

    /// <summary>Non-ASCII title (Polish team names) - must survive the round trip unchanged.</summary>
    public const string Title = "Wisła Kraków – Lech Poznań: symulacja #1";
}

internal static class ContractCulture
{
    /// <summary>
    /// The whole run uses a culture with day-first dates and decimal comma ("14.10.2026", "2,5"),
    /// like a developer machine in Poland. Contracts must not depend on the culture of the process -
    /// in Docker the culture is invariant, which hid such bugs.
    /// </summary>
    [ModuleInitializer]
    internal static void UseDayFirstCulture()
    {
        var culture = new CultureInfo("pl-PL");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}
