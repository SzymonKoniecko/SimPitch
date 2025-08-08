using SportsDataService.API.Services;
using SportsDataService.Infrastructure;
using MediatR;
using SportsDataService.Infrastructure.Logging;
var builder = WebApplication.CreateBuilder(args);




// Dodaj gRPC, jeśli używasz
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(typeof(GetTeamByIdHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAllTeamsQuery).Assembly);

//builder.Logging.ClearProviders();
builder.Logging.AddGrpcLogger(builder.Configuration);
builder.Logging.AddConsole();

var grpcClient = new GrpcCliento("http://logging-service:80");
await grpcClient.LogAsync();
Console.WriteLine($"Received data: {"message"}");


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<TeamGrpcService>();

app.MapGet("/", () => "Use gRPC clients for communication");


app.Run();

// Przykład użycia - wstrzyknij ILogger<T> w dowolnym serwisie lub kontrolerze
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("SportsDataService is starting up.");
logger.LogWarning("This is a sample warning.");
try
{
    throw new InvalidOperationException("Something went wrong!");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during startup.");
}

