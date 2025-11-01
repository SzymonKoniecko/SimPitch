
using EngineService.API;
using EngineService.Infrastructure;
using EngineService.Infrastructure.Logging;
using EngineService.Infrastructure.Middlewares;
using EngineService.Application.Features;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger(ConfigHelper.GetLoggerSourceName());
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddControllers();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatRServices();

builder.Services.AddScoped<GrpcExceptionInterceptor>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});

builder.Services.AddSimulationGrpcClient(ConfigHelper.GetSimulationAddress());
builder.Services.AddStatisticsGrpcClient(ConfigHelper.GetStatisticsAddress());

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(6);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(4);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
