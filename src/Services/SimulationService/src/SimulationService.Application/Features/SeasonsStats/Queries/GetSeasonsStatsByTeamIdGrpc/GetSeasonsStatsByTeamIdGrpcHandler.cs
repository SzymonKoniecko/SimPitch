using System;
using MediatR;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;

public class GetSeasonsStatsByTeamIdGrpcHandler : IRequestHandler<GetSeasonsStatsByTeamIdGrpcQuery, IEnumerable<SeasonStats>>
{
    private readonly ISeasonStatsGrpcClient _seasonStatsGrpcClient;

    public GetSeasonsStatsByTeamIdGrpcHandler(ISeasonStatsGrpcClient seasonStatsGrpcClient)
    {
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
    }

    public async Task<IEnumerable<SeasonStats>> Handle(GetSeasonsStatsByTeamIdGrpcQuery query, CancellationToken cancellationToken)
    {
        var result = await _seasonStatsGrpcClient.GetSeasonsStatsByTeamIdAsync(query.teamId, cancellationToken: cancellationToken);
        return result.Select(x => SeasonStatsMapper.DtoToValueObject(x));
    }
}
