using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using MediatR;
using SportsDataService.Infrastructure.Logging;
using SportsDataService.Infrastructure.Middlewares;
using SportsDataService.Application.Features;
var builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();

builder.Logging.AddGrpcLogger("SportsDataService");
builder.Logging.AddConsole();

// Dodaj gRPC, jeśli używasz
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


app.MapGrpcService<TeamGrpcService>();
app.MapGrpcService<StadiumGrpcService>();

app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();