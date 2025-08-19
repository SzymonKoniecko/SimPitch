using System;
using MediatR;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.Leagues.Query.GetLeagueById;

public class GetLeagueByIdHandler : IRequestHandler<GetLeagueByIdQuery, League>
{
    private readonly ILeagueGrpcClient _leagueGrpcClient;

    public GetLeagueByIdHandler(ILeagueGrpcClient leagueGrpcClient)
    {
        _leagueGrpcClient = leagueGrpcClient;
    }

    public async Task<League> Handle(GetLeagueByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _leagueGrpcClient.GetLeagueByIdAsync(request.leagueId);

        return LeagueMapper.ToDomain(response);
    }
}