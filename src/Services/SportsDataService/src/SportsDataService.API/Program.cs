using SportsDataService.API.Services;
using SportsDataService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Dodaj gRPC, jeśli używasz
builder.Services.AddGrpc();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.MapGrpcService<TeamGrpcService>(); // przykładowy gRPC service

app.MapGet("/", () => "Use gRPC clients for communication");

app.Run();
