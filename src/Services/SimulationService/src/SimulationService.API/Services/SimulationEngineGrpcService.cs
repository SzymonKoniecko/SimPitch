using System;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService;
using SimPitchProtos.SimulationService.SimulationEngine;
using SimulationService.API.Helpers;
using SimulationService.API.Mappers;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Features.Simulations.Commands.RunSimulation.RunSimulationCommand;
using SimulationService.Application.Features.Simulations.Commands.SetSimulation;
using SimulationService.Application.Features.Simulations.Commands.StopSimulation;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationIdsByDate;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviews;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;
using SimulationService.Application.Mappers;
using SimulationOverviewMapper = SimulationService.API.Mappers.SimulationOverviewMapper;
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

    public override async Task<SimulationOverviewsPagedResponse> GetAllSimulationOverviews(PagedRequestGrpc pagedRequest, ServerCallContext serverCallContext)
    {

        var query = new GetAllSimulationOverviewsQuery(
            ProtoHelper.ValidateProtoAndMapToDto(pagedRequest)
        );

        var response = await _mediator.Send(query, serverCallContext.CancellationToken);

        return new SimulationOverviewsPagedResponse
        {
            Items = { response.Item1.Select(x => SimulationOverviewMapper.ToProto(x)) },
            Paged = ProtoHelper.MapPagedResultsToProto(response.Item2)
        };
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

    public override async Task<SimulationIdsResponse> GetLatestSimulationIds(GetLatestSimulationIdsRequest request, ServerCallContext context)
    {
        var query = new GetSimulationIdsByDateQuery(DateTime.Parse(request.Date));

        var response = await _mediator.Send(query, context.CancellationToken);

        return new SimulationIdsResponse
        {
            SimulationIds = { response.Select(x => x)}
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
