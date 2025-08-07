using SportsDataService.API.Extensions;
using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Dodaj gRPC, jeśli używasz
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGrpcClients(builder.Configuration);

builder.Services.AddMediatR(typeof(GetTeamByIdHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAllTeamsQuery).Assembly);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<TeamGrpcService>();

app.MapGet("/", () => "Use gRPC clients for communication");

app.Run();
