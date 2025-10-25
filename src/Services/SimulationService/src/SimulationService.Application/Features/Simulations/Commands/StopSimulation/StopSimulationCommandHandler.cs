using System;
using MediatR;
using SimulationService.Application.Interfaces;

namespace SimulationService.Application.Features.Simulations.Commands.StopSimulation;

public class StopSimulationCommandHandler : IRequestHandler<StopSimulationCommand, string>
{
    private readonly IRedisSimulationRegistry _registry;

    public StopSimulationCommandHandler(IRedisSimulationRegistry registry)
    {
        _registry = registry;
    }

    public async Task<string> Handle(StopSimulationCommand request, CancellationToken cancellationToken)
    {
        // pobierz aktualny stan
        var current = await _registry.GetStateAsync(request.SimulationId, cancellationToken);
        if (current is null)
            throw new InvalidOperationException($"Simulation {request.SimulationId} not found.");

        try
        {
            // ustaw nowy stan
            var newState = current.SetCancelled();

            await _registry.SetStateAsync(request.SimulationId, newState, cancellationToken);
            return "Cancelled";

        }
        catch (System.Exception ex)
        {
            return ex.Message;
        }
    }
}