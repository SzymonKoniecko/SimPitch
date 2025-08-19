using System;
using Grpc.Core;
using MediatR;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.League;
using SportsDataService.Application.Features.League.Queries.GetLeagueById;

namespace SportsDataService.API.Services;

public class LeagueGrpcService : LeagueService.LeagueServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LeagueGrpcService> _logger;

    public LeagueGrpcService(IMediator mediator, ILogger<LeagueGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<LeaguesByCountryIdResponse> GetLeaguesByCountryId(LeaguesByCountryIdRequest request, ServerCallContext context)
    {
        var query = new GetLeaguesByCountryIdQuery(Guid.Parse(request.CountryId));
        var response = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        return new LeaguesByCountryIdResponse
        {
            Leagues = { response.Select(r => LeagueMapper.ToProto(r)) }
        };
    }
    
    public override async Task<LeagueGrpc> GetLeagueById(LeagueByIdRequest request, ServerCallContext context)
    {
        var query = new GetLeagueByIdQuery(Guid.Parse(request.LeagueId));
        var leagueDto = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        if (leagueDto == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"League with ID '{request.LeagueId}' not found."));
        }

        return LeagueMapper.ToProto(leagueDto);
    }
}