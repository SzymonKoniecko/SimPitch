using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Application.Features.Leagues.Query.GetLeagueById;
using SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;
using SimulationService.Application.Features.SimulationResults.Commands.CreateSimulationResultCommand;
using SimulationService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;
using SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
namespace SimulationService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(InitSimulationContentCommand).Assembly);
        services.AddMediatR(typeof(RunSimulationCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateSimulationResultCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(InitSimulationContentCommandHandler).Assembly);
        services.AddMediatR(typeof(RunSimulationCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateSimulationResultCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcQuery).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdQuery).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdQuery).Assembly);
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetLeagueRoundsByParamsGrpcHandler).Assembly);
        services.AddMediatR(typeof(GetLeagueByIdHandler).Assembly);
        services.AddMediatR(typeof(GetMatchRoundsByIdHandler).Assembly);
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQueryHandler).Assembly);

        // Validators
        services.AddTransient<RunSimulationCommandValidator>();

        return services;
    }
}
