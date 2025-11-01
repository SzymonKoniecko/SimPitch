using SimulationService.Infrastructure;
using SimulationService.Infrastructure.Middlewares;
using SimulationService.Infrastructure.Logging;
using SimulationService.Application.Features;
using SimulationService.API.Services;
using SimulationService.API.Helpers;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger(ConfigHelper.GetLoggerSourceName());
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = ConfigHelper.GetRedisCacheConnectionString();
    options.InstanceName = "SimulationCache";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(ConfigHelper.GetRedisCacheConnectionString())
);

builder.Services.AddMediatRServices();

builder.Services.AddScoped<GrpcExceptionInterceptor>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
    options.MaxSendMessageSize = int.MaxValue;
    options.MaxReceiveMessageSize = int.MaxValue;
});

builder.Services.AddSportsDataGrpcClient(ConfigHelper.GetSportsDataAddress());

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<SimulationEngineGrpcService>();
app.MapGrpcService<IterationResultGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
