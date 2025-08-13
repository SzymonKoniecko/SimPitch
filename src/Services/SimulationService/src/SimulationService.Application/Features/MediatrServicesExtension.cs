using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
namespace SimulationService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        
        // Commands handlers

        // Queries
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcQuery).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcHandler).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdHandler).Assembly);

        // Validators

        return services;
    }
}
