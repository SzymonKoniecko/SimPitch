using System;
using SimPitchProtos.SportsDataService.RealMatchResult;
using SportsDataService.Domain.Entities;
using Grpc.Core;
using MediatR;
using SportsDataService.Application.Features.RealMatchResult.Queries.GetRealMatchResultsByRoundId;
using SportsDataService.API.Mappers;

namespace SportsDataService.API.Services;

public class RealMatchResultGrpcService : RealMatchResultService.RealMatchResultServiceBase
{
    private readonly IMediator _mediator;
    public RealMatchResultGrpcService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public override async Task<RealMatchResultsByRoundIdResponse> GetRealMatchResultsByRoundId(RealMatchResultsByRoundIdRequest request, ServerCallContext context)
    {
        var query = new GetRealMatchResultsByRoundIdQuery(Guid.Parse(request.RoundId));
        var realMatchResults = await _mediator.Send(query, context.CancellationToken);

        return new RealMatchResultsByRoundIdResponse
        {
            RealMatchResults = { realMatchResults.Select(r => RealMatchResultMapper.ToProto(r))}
        };
    }
}
