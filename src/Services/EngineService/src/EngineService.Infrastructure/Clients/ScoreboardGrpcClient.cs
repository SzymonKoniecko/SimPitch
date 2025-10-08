using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.Scoreboard;

namespace EngineService.Infrastructure.Clients;

public class ScoreboardGrpcClient : IScoreboardGrpcClient
{
    private readonly ScoreboardService.ScoreboardServiceClient _client;

    public ScoreboardGrpcClient(ScoreboardService.ScoreboardServiceClient scoreboardServiceClient)
    {
        _client = scoreboardServiceClient ?? throw new ArgumentNullException(nameof(scoreboardServiceClient));
    }

    public async Task<List<ScoreboardDto>> GetScoreboardsByQueryAsync(Guid simulationId, CancellationToken cancellationToken, Guid iterationId = default, bool? withTeamStats = null)
    {
        var request = new ScoreboardsByQueryRequest
        {
            SimulationId = simulationId.ToString()
        };

        if (iterationId != Guid.Empty)
        {
            request.IterationResultId = iterationId.ToString();
        }

        if (withTeamStats.HasValue)
        {
            request.WithTeamStats = withTeamStats.Value;
        }

        var response = await _client.GetScoreboardsByQueryAsync(request, cancellationToken: cancellationToken);
        return (List<ScoreboardDto>)response.Scoreboards.Select(x => ProtoToDto(x));
    }

    private static ScoreboardDto ProtoToDto(ScoreboardGrpc grpc)
    {
        var dto = new ScoreboardDto();
        dto.Id =  Guid.Parse(grpc.Id);
        dto.SimulationId =  Guid.Parse(grpc.SimulationId);
        dto.IterationResultId =  Guid.Parse(grpc.IterationResultId);
        dto.ScoreboardTeams = (List<ScoreboardTeamStatsDto>)grpc.ScoreboardTeams.Select(x => ProtoToDto(x));
        dto.LeagueStrength = grpc.LeagueStrength;
        dto.PriorLeagueStrength = grpc.PriorLeagueStrength;
        dto.CreatedAt = DateTime.Parse(grpc.CreatedAt);

        return dto;
    }

    private static ScoreboardTeamStatsDto ProtoToDto(ScoreboardTeamStatsGrpc grpc)
    {
        var dto = new ScoreboardTeamStatsDto();

        dto.Id = Guid.Parse(grpc.Id);
        dto.ScoreboardId = Guid.Parse(grpc.ScoreboardId);
        dto.TeamId = Guid.Parse(grpc.TeamId);
        dto.Rank = grpc.Rank;
        dto.Points = grpc.Points;
        dto.MatchPlayed = grpc.MatchPlayed;
        dto.Wins = grpc.Wins;
        dto.Losses = grpc.Losses;
        dto.Draws = grpc.Draws;
        dto.GoalsFor = grpc.GoalsFor;
        dto.GoalsAgainst = grpc.GoalsAgainst;

        return dto;
    }
}
