using System;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.API.Mappers;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
namespace SimulationService.API.Services;

public class SimulationEngineGrpcService : SimulationEngineService.SimulationEngineServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SimulationEngineGrpcService> _logger;

    public SimulationEngineGrpcService(IMediator mediator, ILogger<SimulationEngineGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<RunSimulationEngineResponse> RunSimulation(RunSimulationEngineRequest request, ServerCallContext context)
    {
        var command = new RunSimulationCommand(SimulationEngineMapper.SimulationEngineReqestToDto(request));

        Guid response = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new RunSimulationEngineResponse
        {
            SimulationId = response.ToString()
        };
    }
}
