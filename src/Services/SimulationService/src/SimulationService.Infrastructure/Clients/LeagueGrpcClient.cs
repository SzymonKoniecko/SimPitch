using System;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.League;
using SimulationService.Application.Features.Leagues.DTOs;
using SimulationService.Application.Interfaces;
using SimulationService.Application.Mappers;

namespace SimulationService.Infrastructure.Clients;

public class LeagueGrpcClient : ILeagueGrpcClient
{
    private readonly LeagueService.LeagueServiceClient _leagueServiceClient;

    public LeagueGrpcClient(LeagueService.LeagueServiceClient leagueServiceClient)
    {
        _leagueServiceClient = leagueServiceClient;
    }

    public async Task<LeagueDto> GetLeagueByIdAsync(Guid leagueId)
    {
        var request = new LeagueByIdRequest { LeagueId = leagueId.ToString() };
        var response = await _leagueServiceClient.GetLeagueByIdAsync(request);

        if (response == null)
        {
            throw new Exception($"League with ID {leagueId} not found.");
        }
        
        return ProtoToDto(response);
    }

    private static LeagueDto ProtoToDto(LeagueGrpc grpc)
    {
        return new LeagueDto
        {
            Id = Guid.Parse(grpc.Id),
            Name = grpc.Name,
            CountryId = Guid.Parse(grpc.CountryId),
            MaxRound = grpc.MaxRound,
            Strengths = grpc.LeagueStrengths.Select(x => ToProto(x)).ToList()
        };
    }
    private static LeagueStrengthDto ToProto(LeagueStrengthGrpc grpc)
    {
        return new LeagueStrengthDto
        {
            Id = Guid.Parse(grpc.Id),
            LeagueId = Guid.Parse(grpc.LeagueId),
            SeasonYear = grpc.SeasonYear,
            Strength = grpc.Strength
        };
    }
}