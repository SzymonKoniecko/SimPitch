using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
namespace SimulationService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        
        // Commands handlers

        // Queries
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcHandler).Assembly);

        // Validators

        return services;
    }
}
