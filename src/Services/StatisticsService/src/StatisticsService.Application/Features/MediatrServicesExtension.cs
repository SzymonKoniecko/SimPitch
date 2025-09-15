using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

namespace StatisticsService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(CreateScoreboardCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(CreateScoreboardCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQueryHandler).Assembly);

        // Validators

        return services;
    }
}
