using System;
using MediatR;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Interfaces;

namespace SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;

public class GetMatchRoundsByIdHandler : IRequestHandler<GetMatchRoundsByIdQuery, List<MatchRoundDto>>
{
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;

    public GetMatchRoundsByIdHandler(IMatchRoundGrpcClient matchRoundGrpcClient)
    {
        _matchRoundGrpcClient = matchRoundGrpcClient;
    }

    public async Task<List<MatchRoundDto>> Handle(GetMatchRoundsByIdQuery query, CancellationToken cancellationToken)
    {
        return await _matchRoundGrpcClient.GetMatchRoundsByRoundId(query.RoundId, cancellationToken);
    }
}
