using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.Scoreboard;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

namespace StatisticsService.API.Services;

public class ScoreboardGrpcService : ScoreboardService.ScoreboardServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ScoreboardGrpcService> _logger;

    public ScoreboardGrpcService(IMediator mediatr, ILogger<ScoreboardGrpcService> logger)
    {
        _mediator = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<ScoreboardsResponse> CreateScoreboards(CreateScoreboardsRequest request, ServerCallContext context)
    {
        var command = new CreateScoreboardCommand(Guid.Parse(request.SimulationId), request.HasIterationResultId ? Guid.Parse(request.IterationResultId): Guid.Empty);

        var results = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new ScoreboardsResponse
        {
            Scoreboards = { results.Select(x => ScoreboardToGrpc(x)) }
        };
    }

    public override async Task<ScoreboardsResponse> GetScoreboardsByQuery(ScoreboardsByQueryRequest request, ServerCallContext context)
    {
        var query = new GetScoreboardsBySimulationIdQuery(
            Guid.Parse(request.SimulationId),
            request.HasIterationResultId ? Guid.Parse(request.IterationResultId): Guid.Empty,
            request.WithTeamStats
        );

        var results = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        if (results != null && results.Count > 0)
        {
            return new ScoreboardsResponse
            {
                Scoreboards = { results.Select(x => ScoreboardToGrpc(x)) }
            };
        }
        // Flow like: CreateScoreboards(CreateScoreboardsRequest request, ServerCallContext context)
        var command = new CreateScoreboardCommand(Guid.Parse(request.SimulationId), request.HasIterationResultId ? Guid.Parse(request.IterationResultId): Guid.Empty);

        var createdResults = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new ScoreboardsResponse
        {
            Scoreboards = { createdResults.Select(x => ScoreboardToGrpc(x)) }
        };
    }


    private static ScoreboardGrpc ScoreboardToGrpc(ScoreboardDto dto)
    {
        var grpc = new ScoreboardGrpc
        {
            Id = dto.Id.ToString(),
            SimulationId = dto.SimulationId.ToString(),
            IterationResultId = dto.IterationResultId.ToString(),
            ScoreboardTeams = { dto.ScoreboardTeams.Select(team => ScoreboardTeamStatsToGrpc(team)).OrderBy(x => x.Rank).ToList() },
            LeagueStrength = dto.LeagueStrength,
            PriorLeagueStrength = dto.PriorLeagueStrength,
            CreatedAt = dto.CreatedAt.ToString()
        };

        return grpc;
    }

    public static ScoreboardTeamStatsGrpc ScoreboardTeamStatsToGrpc(ScoreboardTeamStatsDto dto)
    {
        var grpc = new ScoreboardTeamStatsGrpc();

        grpc.Id = dto.Id.ToString();
        grpc.ScoreboardId = dto.ScoreboardId.ToString();
        grpc.TeamId = dto.TeamId.ToString();
        grpc.Rank = dto.Rank;
        grpc.Points = dto.Points;
        grpc.MatchPlayed = dto.MatchPlayed;
        grpc.Wins = dto.Wins;
        grpc.Losses = dto.Losses;
        grpc.Draws = dto.Draws;
        grpc.GoalsFor = dto.GoalsFor;
        grpc.GoalsAgainst = dto.GoalsAgainst;

        return grpc;
    }
}
