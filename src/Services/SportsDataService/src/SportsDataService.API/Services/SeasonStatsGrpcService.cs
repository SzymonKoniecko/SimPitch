using System;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SportsDataService.SeasonStats;
using SportsDataService.API.Mappers;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonsStatsByLeagueAndSeasonYear;
using SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;

namespace SportsDataService.API.Services;

public class SeasonStatsGrpcService : SeasonStatsService.SeasonStatsServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SeasonStatsGrpcService> _logger;

    public SeasonStatsGrpcService(IMediator mediator, ILogger<SeasonStatsGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<SeasonStatsByTeamIdResponse> GetSeasonsStatsByTeamId(SeasonStatsByTeamIdRequest request, ServerCallContext context)
    {
        var query = new GetSeasonsStatsByTeamIdQuery(Guid.Parse(request.TeamId));
        var response = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        return new SeasonStatsByTeamIdResponse
        {
            SeasonsStats = { response.Select(x => SeasonStatsMapper.ToProto(x)) }
        };
    }

    public override async Task<SeasonStatsByLeagueAndSeasonYearResponse> GetSeasonStatsByLeagueAndSeasonYear(SeasonStatsByLeagueAndSeasonYearRequest request, ServerCallContext context)
    {
        var query = new GetSeasonsStatsByLeagueAndSeasonYearQuery(Guid.Parse(request.LeagueId), request.SeasonYear);
        var response = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        return new SeasonStatsByLeagueAndSeasonYearResponse
        {
            SeasonsStats = { response.Select(x => SeasonStatsMapper.ToProto(x)) }
        };
    }
}
