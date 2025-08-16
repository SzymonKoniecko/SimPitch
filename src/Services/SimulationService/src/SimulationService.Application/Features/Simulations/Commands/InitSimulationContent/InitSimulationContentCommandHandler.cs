using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Services;

namespace SimulationService.Application.Features.Simulations.Commands.InitSimulationContent;

public class InitSimulationContentCommandHandler : IRequestHandler<InitSimulationContentCommand, List<MatchRound>>
{
    private readonly SeasonStatsService _seasonStatsService;
    private readonly IMediator _mediator;
    

    public InitSimulationContentCommandHandler(SeasonStatsService seasonStatsService, IMediator mediator)
    {
        _seasonStatsService = seasonStatsService ?? throw new ArgumentNullException(nameof(seasonStatsService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<List<MatchRound>> Handle(InitSimulationContentCommand query, CancellationToken cancellationToken)
    {
        List<MatchRound> matchRounds = new();
        LeagueRoundDtoRequest leagueRoundDtoRequest = new();

        leagueRoundDtoRequest.SeasonYear = query.SimulationParamsDto.SeasonYear;
        leagueRoundDtoRequest.LeagueRoundId = query.SimulationParamsDto.RoundId;

        var leagueRounds = await _mediator.Send(new GetLeagueRoundsByParamsGrpcQuery(leagueRoundDtoRequest));


        return null;
    }
}
