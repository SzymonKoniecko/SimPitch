using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Simulations.Commands.CreateSimulation;

public class CreateSimulationCommandHandler : IRequestHandler<CreateSimulationCommand, string>
{
    private readonly ISimulationEngineGrpcClient _simulationEngineGrpcClient;

    public CreateSimulationCommandHandler(ISimulationEngineGrpcClient simulationEngineGrpcClient)
    {
        _simulationEngineGrpcClient = simulationEngineGrpcClient;
    }

    public async Task<string> Handle(CreateSimulationCommand command, CancellationToken cancellationToken)
    {
        string simulationId = await _simulationEngineGrpcClient.CreateSimulationAsync(command.simulationParamsDto, cancellationToken: cancellationToken);
        
        return simulationId;
    }
}
