using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

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
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQueryHandler).Assembly);

        // Validators

        return services;
    }
}
