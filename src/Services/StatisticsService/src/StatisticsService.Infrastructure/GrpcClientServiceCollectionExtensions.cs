using System;
using Microsoft.Extensions.DependencyInjection;
using SimPitchProtos.SimulationService.SimulationResult;

namespace StatisticsService.Infrastructure;

public static class GrpcClientServiceCollectionExtensions
{
    public static IServiceCollection AddSimulationGrpcClient(this IServiceCollection services, string simulationServiceAddress)
    {

        services.AddGrpcClient<SimulationResultService.SimulationResultServiceClient>(options =>
        {
            options.Address = new Uri(simulationServiceAddress);
        });

        
        return services;
    }
}
