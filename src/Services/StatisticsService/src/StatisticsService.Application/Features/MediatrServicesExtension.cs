using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using StatisticsService.Application.Features.SimulationStats.Commands;
using StatisticsService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;

namespace StatisticsService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        services.AddMediatR(typeof(CreateScoreboardCommand).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommand).Assembly);
        services.AddMediatR(typeof(CreateSimulationStatsCommand).Assembly);
        
        // Commands handlers
        services.AddMediatR(typeof(CreateScoreboardCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateScoreboardByIterationResultCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateSimulationStatsCommandHandler).Assembly);

        // Queries
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQuery).Assembly);
        services.AddMediatR(typeof(GetSimulationStatsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetIterationResultsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetScoreboardsBySimulationIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetSimulationStatsBySimulationIdQueryHandler).Assembly);

        // Validators

        return services;
    }
}
