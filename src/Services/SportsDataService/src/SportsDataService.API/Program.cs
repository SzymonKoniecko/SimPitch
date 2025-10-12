using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using SportsDataService.Infrastructure.Logging;
using SportsDataService.Infrastructure.Middlewares;
using SportsDataService.Application.Features;
using SportsDataService.API;

var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger(ConfigHelper.GetLoggerSourceName());
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatRServices();

builder.Services.AddScoped<GrpcExceptionInterceptor>();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
});

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