using System;
using EngineService.Application.DTOs;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardByLeagueIdAndSeasonYear;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.Scoreboards.Queries;

public class GetScoreboardByLeagueIdAndSeasonYearQueryHandler : IRequestHandler<GetScoreboardByLeagueIdAndSeasonYearQuery, ScoreboardDto>
{
    private readonly IScoreboardGrpcClient _scoreboardGrpcClient;

    public GetScoreboardByLeagueIdAndSeasonYearQueryHandler(IScoreboardGrpcClient scoreboardGrpcClient)
    {
        _scoreboardGrpcClient = scoreboardGrpcClient;
    }

    public async Task<ScoreboardDto> Handle(GetScoreboardByLeagueIdAndSeasonYearQuery query, CancellationToken cancellationToken)
    {
        return await _scoreboardGrpcClient.CreateScoreboardByLeagueIdAndSeasonYear(query.leagueId, query.seasonYear, cancellationToken);
    }
}
