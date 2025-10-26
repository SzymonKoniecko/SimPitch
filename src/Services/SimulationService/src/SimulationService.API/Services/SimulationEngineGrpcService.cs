using System;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.API.Mappers;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Features.Simulations.Commands.SetSimulation;
using SimulationService.Application.Features.Simulations.Commands.StopSimulation;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;
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
        var command = new SetSimulationCommand(SimulationEngineMapper.SimulationEngineReqestToDto(request));

        Guid response = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new RunSimulationEngineResponse
        {
            SimulationId = response.ToString()
        };
    }

    public override async Task<SimulationOverviewResponse> GetSimulationOverviewById(GetSimulationOverviewByIdRequest request, ServerCallContext serverCallContext)
    {
        var query = new GetSimulationOverviewByIdQuery(Guid.Parse(request.SimulationId));

        var response = await _mediator.Send(query, serverCallContext.CancellationToken);

        return new SimulationOverviewResponse
        {
            SimulationOverview = SimulationOverviewMapper.ToProto(response)
        };
    }

    public override async Task<SimulationOverviewsListResponse> GetAllSimulationOverviews(Empty empty, ServerCallContext serverCallContext)
    {
        var query = new GetAllSimulationOverviewsQuery();

        var response = await _mediator.Send(query, serverCallContext.CancellationToken);

        var result = new SimulationOverviewsListResponse();
        result.SimulationOverviews.AddRange(response.Select(x => SimulationOverviewMapper.ToProto(x)));
        return result;
    }

    public override async Task<SimulationStateResponse> GetSimulationStateById(GetSimulationStateByIdRequest request, ServerCallContext context)
    {
        var query = new GetSimulationStateBySimulationIdQuery(Guid.Parse(request.SimulationId));

        var response = await _mediator.Send(query, context.CancellationToken);

        return new SimulationStateResponse
        {
            SimulationState = SimulationEngineMapper.StateToGrpc(response)
        };
    }
    
    public override async Task<StopSimulationResponse> StopSimulationById(StopSimulationRequest request, ServerCallContext context)
    {
        var command = new StopSimulationCommand(Guid.Parse(request.SimulationId));

        var response = await _mediator.Send(command, context.CancellationToken);

        return new StopSimulationResponse
        {
            Status = response.ToString()
        };
    }
}
