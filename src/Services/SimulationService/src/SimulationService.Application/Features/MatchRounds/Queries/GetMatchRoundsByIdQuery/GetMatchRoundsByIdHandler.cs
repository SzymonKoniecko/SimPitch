using System;
using MediatR;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;

public class GetMatchRoundsByIdHandler : IRequestHandler<GetMatchRoundsByIdQuery, List<MatchRound>>
{
    private readonly IMatchRoundGrpcClient _matchRoundGrpcClient;

    public GetMatchRoundsByIdHandler(IMatchRoundGrpcClient matchRoundGrpcClient)
    {
        _matchRoundGrpcClient = matchRoundGrpcClient;
    }

    public async Task<List<MatchRound>> Handle(GetMatchRoundsByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _matchRoundGrpcClient.GetMatchRoundsByRoundId(query.RoundId, cancellationToken);
        return result.Select(x => MatchRoundMapper.ToDomain(x)).ToList();
    }
}
