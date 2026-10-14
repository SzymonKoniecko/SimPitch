using Grpc.Net.Client;
using SimPitch.Testing;
using SimPitchProtos.SimulationService.IterationResult;
using SimPitchProtos.SimulationService.SimulationEngine;

namespace SimulationService.Tests.ApiTests.Infrastructure;

[CollectionDefinition(Name)]
public class SimulationApiCollection : ICollectionFixture<SimulationApiFixture>
{
    public const string Name = "SimulationService API";
}

/// <summary>
/// Starts SQL Server + Redis containers once for all API tests and hosts SimulationService against them.
/// The tests talk to the service over real gRPC (through the in-memory test server).
/// </summary>
public sealed class SimulationApiFixture : IAsyncLifetime
{
    public SimulationStackContainers Containers { get; } = new();
    public SimulationServiceFactory Factory { get; private set; } = default!;
    public SimulationEngineService.SimulationEngineServiceClient Engine { get; private set; } = default!;
    public IterationResultService.IterationResultServiceClient Iterations { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await Containers.StartAsync();
        Containers.ApplyEnvironment();

        Factory = new SimulationServiceFactory();
        var channel = GrpcChannel.ForAddress(Factory.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpHandler = Factory.Server.CreateHandler()
        });
        Engine = new SimulationEngineService.SimulationEngineServiceClient(channel);
        Iterations = new IterationResultService.IterationResultServiceClient(channel);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Containers.DisposeAsync();
    }
}
