using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using MediatR;
using SportsDataService.Infrastructure.Logging;
using SportsDataService.Infrastructure.Middlewares;
var builder = WebApplication.CreateBuilder(args);



// Czyścimy wbudowanych providerów
builder.Logging.ClearProviders();

// Dodajemy gRPC loggera
builder.Logging.AddGrpcLogger("SportsDataService");
builder.Logging.AddConsole();

// Dodaj gRPC, jeśli używasz
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(typeof(GetTeamByIdHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAllTeamsQuery).Assembly);

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

app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();