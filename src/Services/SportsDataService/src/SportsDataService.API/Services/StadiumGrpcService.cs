using System;
using SportsDataService.Domain.Entities;
using SimPitchProtos.SportsDataService.Stadium;
using SimPitchProtos.SportsDataService;
using MediatR;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SportsDataService.Application.Features.Stadium.Queries.GetAllStadiums;
using SportsDataService.Application.Features.Stadium.Queries.GetStadiumById;
using SportsDataService.Application.Features.Stadium.Commands.CreateStadium;

namespace SportsDataService.API.Services;

public class StadiumGrpcService : StadiumService.StadiumServiceBase
{
    private readonly IMediator _mediator;

    public StadiumGrpcService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public override async Task<StadiumListResponse> GetAllStadiums(Empty request, ServerCallContext context)
    {
        var query = new GetAllStadiumsQuery();
        var stadiums = await _mediator.Send(query, context.CancellationToken);
        return new StadiumListResponse
        {
            Stadiums = { stadiums.Select(s => StadiumMapper.ToProto(s)) }
        };
    }
    public override async Task<StadiumGrpc> GetStadiumById(StadiumByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var guid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));

        var query = new GetStadiumByIdQuery(guid);
        var stadium = await _mediator.Send(query, context.CancellationToken);

        if (stadium == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Stadium not found"));

        return stadium.ToProto();
    }
    public override async Task<StadiumIdResponse> CreateStadium(CreateStadiumRequest request, ServerCallContext context)
    {
        if (request == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request cannot be null"));

        var command = new CreateStadiumCommand(request.ToDto());
        var stadiumId = await _mediator.Send(command, context.CancellationToken);

        return new StadiumIdResponse { Id = stadiumId.ToString() };
    }
}