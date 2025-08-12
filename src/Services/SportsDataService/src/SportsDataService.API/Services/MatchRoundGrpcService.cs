using System;
using SimPitchProtos.SportsDataService.MatchRound;
using SportsDataService.Domain.Entities;
using Grpc.Core;
using MediatR;
using SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;
using SportsDataService.API.Mappers;

namespace SportsDataService.API.Services;

public class MatchRoundGrpcService : MatchRoundService.MatchRoundServiceBase
{
    private readonly IMediator _mediator;
    public MatchRoundGrpcService(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public override async Task<MatchRoundsByRoundIdResponse> GetMatchRoundsByRoundId(MatchRoundsByRoundIdRequest request, ServerCallContext context)
    {
        var query = new GetMatchRoundsByRoundIdQuery(Guid.Parse(request.RoundId));
        var MatchRounds = await _mediator.Send(query, context.CancellationToken);

        return new MatchRoundsByRoundIdResponse
        {
            MatchRounds = { MatchRounds.Select(r => MatchRoundMapper.ToProto(r))}
        };
    }
}
