using StatisticsService.API;
using StatisticsService.Infrastructure;
using StatisticsService.Application.Features;
using StatisticsService.Infrastructure.Logging;
using StatisticsService.Infrastructure.Middlewares;
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

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}


app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();
