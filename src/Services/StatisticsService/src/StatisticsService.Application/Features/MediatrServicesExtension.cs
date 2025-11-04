using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

namespace StatisticsService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(CreateScoreboardCommand).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(CreateScoreboardCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommandHandler).Assembly);

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
