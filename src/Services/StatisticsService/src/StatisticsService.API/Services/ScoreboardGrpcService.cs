using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.Scoreboard;
using StatisticsService.API.Helpers;
using StatisticsService.Application.Consts;
using StatisticsService.Application.DTOs;
using StatisticsService.Application.DTOs.Clients;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByIterationResult;
using StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboardByLeagueIdAndSeasonYear;
using StatisticsService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using StatisticsService.Domain.ValueObjects;

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
        var command = new CreateScoreboardCommand(Guid.Parse(request.SimulationId), request.HasIterationResultId ? Guid.Parse(request.IterationResultId) : Guid.Empty);

        var results = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new ScoreboardsResponse
        {
            Scoreboards = { results.Select(x => ScoreboardToGrpc(x)) }
        };
    }

    public override async Task GetScoreboardsByQuery(
        ScoreboardsByQueryRequest request,
        IServerStreamWriter<ScoreboardsResponse> responseStream,
        ServerCallContext context)
    {
        var simulationId = Guid.Parse(request.SimulationId);
        var iterationResultId = request.HasIterationResultId
            ? Guid.Parse(request.IterationResultId)
            : Guid.Empty;

        var query = new GetScoreboardsBySimulationIdQuery(
            simulationId,
            iterationResultId,
            request.WithTeamStats
        );

        var results = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        // if no results – create scoreboard
        if (results == null || results.Count == 0)
        {
            var command = new CreateScoreboardCommand(simulationId, iterationResultId);
            results = (await _mediator.Send(command, cancellationToken: context.CancellationToken)).ToList();
        }

        await GrpcStreamHelper.StreamListAsync(
            results.Select(ScoreboardToGrpc),
            responseStream,
            items => new ScoreboardsResponse { Scoreboards = { items } },
            chunkSizeBytes: GrpcConsts.CHUNK_SIZE,
            context.CancellationToken
        );
    }

    public override async Task<CreateScoreboardByIterationResultDataResponse> CreateScoreboardByIterationResultData(CreateScoreboardByIterationResultDataRequest request, ServerCallContext context)
    {
        var command = new CreateScoreboardByIterationResultCommand(
            JsonConvert.DeserializeObject<SimulationOverview>(request.SimulationOverviewJson),
            JsonConvert.DeserializeObject<IterationResultDto>(request.IterationResultJson));

        var result = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new CreateScoreboardByIterationResultDataResponse
        {
            IsCreated = result
        };
    }

    public override async Task<CreateScoreboardByLeagueIdAndSeasonYearResponse> CreateScoreboardByLeagueIdAndSeasonYear(CreateScoreboardByLeagueIdAndSeasonYearRequest request, ServerCallContext context)
    {
        var command = new CreateScoreboardByLeagueIdAndSeasonYearCommand(Guid.Parse(request.LeagueId), request.SeasonYear);

        var result = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new CreateScoreboardByLeagueIdAndSeasonYearResponse
        {
            Scoreboard = ScoreboardToGrpc(result)
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
            InitialScoreboardTeams = { dto.InitialScoreboardTeams.Select(team => ScoreboardTeamStatsToGrpc(team)).OrderBy(x => x.Rank).ToList() },
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
        grpc.IsInitialStats = dto.IsInitialStat;

        return grpc;
    }
}
