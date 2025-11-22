using System;
using MediatR;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;

public class GetSeasonsStatsByTeamIdGrpcHandler : IRequestHandler<GetSeasonsStatsByTeamIdGrpcQuery, List<SeasonStats>>
{
    private readonly ISeasonStatsGrpcClient _seasonStatsGrpcClient;
    private readonly ILeagueGrpcClient _leagueGrpcClient;
    public GetSeasonsStatsByTeamIdGrpcHandler(ISeasonStatsGrpcClient seasonStatsGrpcClient, ILeagueGrpcClient leagueGrpcClient)
    {
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
        _leagueGrpcClient = leagueGrpcClient;
    }

    public async Task<List<SeasonStats>> Handle(GetSeasonsStatsByTeamIdGrpcQuery query, CancellationToken cancellationToken)
    {
        var response = await _seasonStatsGrpcClient.GetSeasonsStatsByTeamIdAsync(query.teamId, cancellationToken: cancellationToken);
        List<LeagueDto> leagues = new();
        List<SeasonStats> result = new();
        
        foreach (var seasonStats in response)
        {
            if (!leagues.Any(x => x.Id == seasonStats.LeagueId))
                leagues.Add(await _leagueGrpcClient.GetLeagueByIdAsync(seasonStats.LeagueId));
            result.Add(
                SeasonStatsMapper.DtoToValueObject(
                    seasonStats, 
                    (float)(leagues
                        .First(x => x.Id == seasonStats.LeagueId)?.Strengths
                        .First(x => EnumMapper.StringtoSeasonEnum(x.SeasonYear) == seasonStats.SeasonYear).Strength)
            ));
        }
        return result;
    }
}
