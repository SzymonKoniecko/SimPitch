using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.SeasonStats;
using SimulationService.Application.Features.SeasonsStats.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;

namespace SimulationService.Infrastructure.Clients;

public class SeasonStatsGrpcClient : ISeasonStatsGrpcClient
{
    private readonly SeasonStatsService.SeasonStatsServiceClient _seasonStatsGrpcClient;

    public SeasonStatsGrpcClient(SeasonStatsService.SeasonStatsServiceClient seasonStatsGrpcClient)
    {
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
    }

    public async Task<List<SeasonStatsDto>> GetSeasonsStatsByTeamIdAsync(Guid teamId, CancellationToken cancellationToken)
    {
        var grpcRequest = new SeasonStatsByTeamIdRequest()
        {
            TeamId = teamId.ToString()
        };

        var response = await _seasonStatsGrpcClient.GetSeasonsStatsByTeamIdAsync(grpcRequest, cancellationToken: cancellationToken);
        List<SeasonStatsDto> result = new();
        result.AddRange(response.SeasonsStats?.Select(x => ProtoToDto(x)));

        return result;
    }
    
    private static SeasonStatsDto ProtoToDto(SeasonStatsGrpc grpc) {

        var dto = new SeasonStatsDto();
        dto.Id = Guid.Parse(grpc.Id);
        dto.TeamId = Guid.Parse(grpc.TeamId);
        dto.SeasonYear = EnumMapper.StringtoSeasonEnum(grpc.SeasonYear);
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.MatchesPlayed = grpc.MatchesPlayed;
        dto.Wins = grpc.Wins;
        dto.Losses = grpc.Losses;
        dto.Draws = grpc.Draws;
        dto.GoalsFor = grpc.GoalsFor;
        dto.GoalsAgainst = grpc.GoalsAgainst;

        return dto;
    }
}
