using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Connections;
using SportsDataService.API.Services;
using SportsDataService.Application.Interfaces;
using SportsDataService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Dodaj gRPC, jeśli używasz
builder.Services.AddGrpc();

builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();


var app = builder.Build();

app.MapGrpcService<TeamGrpcService>(); // przykładowy gRPC service

app.MapGet("/", () => "Use gRPC clients for communication");

app.Run();
