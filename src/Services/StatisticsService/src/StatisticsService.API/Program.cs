using StatisticsService.API;
using StatisticsService.Infrastructure;
using StatisticsService.Application.Features;
using StatisticsService.Infrastructure.Logging;
using StatisticsService.Infrastructure.Middlewares;
using StatisticsService.API.Services;
var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger(ConfigHelper.GetLoggerSourceName());
builder.Logging.AddConsole();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatRServices();

builder.Services.AddScoped<GrpcExceptionInterceptor>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});

builder.Services.AddSimulationGrpcClient(ConfigHelper.GetSimulationServiceAddress());
builder.Services.AddSportsDataGrpcClient(ConfigHelper.GetSportsDataServiceAddress());

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<ScoreboardGrpcService>();
app.MapGrpcService<SimulationStatsGrpcService>();

app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();