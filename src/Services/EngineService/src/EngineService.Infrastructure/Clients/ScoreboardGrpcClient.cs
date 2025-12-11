using System;
using System.Globalization;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using Grpc.Core;
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
        var results = new List<ScoreboardDto>();
        int totalCount = 0;

        using var call = _client.GetScoreboardsByQuery(request, cancellationToken: cancellationToken);


        await foreach (var response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            if (response?.Scoreboards != null)
            {
                var mapped = response.Scoreboards.Select(x => ProtoToDto(x));
                results.AddRange(mapped);
            }
        }
        return (
            results
        );
    }

    public async Task<ScoreboardDto> CreateScoreboardByLeagueIdAndSeasonYear(Guid leagueId, string seasonYear, CancellationToken cancellationToken)
    {
        
        var request = new CreateScoreboardByLeagueIdAndSeasonYearRequest
        {
            LeagueId = leagueId.ToString(),
            SeasonYear = seasonYear
        };

        var response = await _client.CreateScoreboardByLeagueIdAndSeasonYearAsync(request, cancellationToken: cancellationToken);

        return ProtoToDto(response.Scoreboard);
    }

    private static ScoreboardDto ProtoToDto(ScoreboardGrpc grpc)
    {
        var dto = new ScoreboardDto();
        dto.Id =  Guid.Parse(grpc.Id);
        dto.SimulationId =  Guid.Parse(grpc.SimulationId);
        dto.IterationResultId =  Guid.Parse(grpc.IterationResultId);
        dto.ScoreboardTeams = grpc.ScoreboardTeams.Select(x => ProtoToDto(x)).ToList();
        dto.InitialScoreboardTeams = grpc.InitialScoreboardTeams.Select(x => ProtoToDto(x)).ToList();
        dto.CreatedAt = DateTime.ParseExact(
            grpc.CreatedAt,
            "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture
        );

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
