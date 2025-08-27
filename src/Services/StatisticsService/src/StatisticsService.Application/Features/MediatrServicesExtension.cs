using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

namespace StatisticsService.Application.Features;

public static class MediatrServicesExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // Commands
        
        // Commands handlers

        // Queries
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQuery).Assembly);

        // Query Handlers
        services.AddMediatR(typeof(GetSimulationResultsBySimulationIdQueryHandler).Assembly);

        // Validators

        return services;
    }
}
