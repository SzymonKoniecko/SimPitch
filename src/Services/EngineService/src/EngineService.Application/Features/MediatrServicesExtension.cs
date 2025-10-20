using EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using EngineService.Application.Features.Simulations.Commands.CreateSimulation;
using EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;
using EngineService.Application.Features.Simulations.Queries.GetSimulationById;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace EngineService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(CreateSimulationCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(CreateSimulationCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetAllSimulationOverviewsQuery).Assembly);
        services.AddMediatR(typeof(GetSimulationByIdQuery).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetAllSimulationOverviewsQueryHandler).Assembly);
        services.AddMediatR(typeof(GetSimulationByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQueryHandler).Assembly);

        // Validators

        return services;
    }
}
