using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.SeasonStats;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Domain.Interfaces;
using StatisticsService.Domain.ValueObjects;

namespace SimulationService.Infrastructure.Clients;

public class SeasonStatsGrpcClient : ISeasonStatsGrpcClient
{
    private readonly SeasonStatsService.SeasonStatsServiceClient _seasonStatsGrpcClient;

    public SeasonStatsGrpcClient(SeasonStatsService.SeasonStatsServiceClient seasonStatsGrpcClient)
    {
        _seasonStatsGrpcClient = seasonStatsGrpcClient;
    }

    public async Task<List<SeasonStats>> GetSeasonStatsByLeagueAndSeasonYear(Guid leagueId, string seasonYear, CancellationToken cancellationToken)
    {
        var grpcRequest = new SeasonStatsByLeagueAndSeasonYearRequest()
        {
            LeagueId = leagueId.ToString(),
            SeasonYear = seasonYear
        };

        var response = await _seasonStatsGrpcClient.GetSeasonStatsByLeagueAndSeasonYearAsync(grpcRequest, cancellationToken: cancellationToken);

        return response.SeasonsStats?.Select(x => ProtoToVo(x)).ToList();
    }
    
    private static SeasonStats ProtoToVo(SeasonStatsGrpc grpc) {

        var Vo = new SeasonStats();
        Vo.Id = Guid.Parse(grpc.Id);
        Vo.TeamId = Guid.Parse(grpc.TeamId);
        Vo.SeasonYear = grpc.SeasonYear;
        Vo.LeagueId = Guid.Parse(grpc.LeagueId);
        Vo.MatchesPlayed = grpc.MatchesPlayed;
        Vo.Wins = grpc.Wins;
        Vo.Losses = grpc.Losses;
        Vo.Draws = grpc.Draws;
        Vo.GoalsFor = grpc.GoalsFor;
        Vo.GoalsAgainst = grpc.GoalsAgainst;

        return Vo;
    }
}
