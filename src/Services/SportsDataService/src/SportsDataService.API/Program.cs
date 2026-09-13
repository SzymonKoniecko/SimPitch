using Elastic.Apm.NetCoreAll;
using Elastic.Apm.StackExchange.Redis;
using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using SportsDataService.Infrastructure.Logging;
using SportsDataService.Infrastructure.Middlewares;
using SportsDataService.Application.Features;
using SportsDataService.API;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger(ConfigHelper.GetLoggerSourceName());
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = ConfigHelper.GetRedisCacheConnectionString();
    options.InstanceName = "SportsDataCache";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connection = ConnectionMultiplexer.Connect(ConfigHelper.GetRedisCacheConnectionString());
    connection.UseElasticApm();
    return connection;
});

builder.Services.AddMediatRServices();

builder.Services.AddScoped<GrpcExceptionInterceptor>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});

builder.Services.AddAllElasticApm();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();

app.MapGrpcService<LeagueGrpcService>();
app.MapGrpcService<LeagueRoundGrpcService>();
app.MapGrpcService<MatchRoundGrpcService>();
app.MapGrpcService<SeasonStatsGrpcService>();
app.MapGrpcService<StadiumGrpcService>();
app.MapGrpcService<TeamGrpcService>();

app.MapControllers();
app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();

public partial class Program { }