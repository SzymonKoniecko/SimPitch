using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EngineService.Application.Features.Simulations.Commands.StopSimulation;

public class StopSimulationCommandHandler : IRequestHandler<StopSimulationCommand, SimulationStateDto>
{
    private readonly ILogger<StopSimulationCommandHandler> _logger;
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public StopSimulationCommandHandler(ILogger<StopSimulationCommandHandler> logger, ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _logger = logger;
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }

    public async Task<SimulationStateDto> Handle(StopSimulationCommand request, CancellationToken cancellationToken)
    {
        var responseStatus = await _simulationEngineGrpcClient.StopSimulationAsync(request.SimulationId, cancellationToken);
        var currentState = await _simulationEngineGrpcClient.GetSimulationStateAsync(request.SimulationId, cancellationToken);

        if (responseStatus.Equals(currentState.State))
        {
            return currentState;
        }
        throw new Exception("State is not equal to stop simulation response!");
    }
}