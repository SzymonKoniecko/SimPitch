using System;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using SimPitchProtos.SportsDataService;
using SimPitchProtos.SportsDataService.LeagueRound;
using StatisticsService.Application.Features.LeagueRounds.DTOs;
using StatisticsService.Application.Interfaces;

namespace StatisticsService.Infrastructure.Clients;

public class LeagueRoundGrpcClient : ILeagueRoundGrpcClient
{
    private readonly LeagueRoundService.LeagueRoundServiceClient _leagueRoundClient;
    private readonly ILogger<LeagueRoundGrpcClient> _logger;

    public LeagueRoundGrpcClient(LeagueRoundService.LeagueRoundServiceClient leagueRoundClient, ILogger<LeagueRoundGrpcClient> logger)
    {
        _leagueRoundClient = leagueRoundClient;
        _logger = logger;
    }

    public async Task<List<LeagueRoundDto>> GetAllLeagueRoundsByParams(LeagueRoundDtoRequest request, CancellationToken cancellationToken)
    {
        var result = new List<LeagueRoundDto>();
        foreach (var seasonYear in request.SeasonYears)
        {
            LeagueRoundsByParamsResponse response = new();
            var grpcRequest = new LeagueRoundsByParamsRequest
            {
                SeasonYear = seasonYear,
                LeagueId = request.LeagueId.ToString()
            };
            
            if (request.LeagueRoundId != Guid.Empty)
                grpcRequest.LeagueRoundId = request.LeagueRoundId.ToString();

            try
            {
                response = await _leagueRoundClient.GetAllLeagueRoundsByParamsAsync(grpcRequest, cancellationToken: cancellationToken);
            }
            catch (RpcException rpc) when (rpc.StatusCode == StatusCode.NotFound)
            {
                _logger.LogWarning($"LeagueRounds not found for SeasonYear={seasonYear}, LeagueId={request.LeagueId}", 
                        grpcRequest.SeasonYear, grpcRequest.LeagueId);
            }
            result.AddRange(response.LeagueRounds?.Select(lr => ProtoToDto(lr)));
        }
        return result;
    }

    private static LeagueRoundDto ProtoToDto(LeagueRoundGrpc grpc)
    {
        var dto = new LeagueRoundDto();
        dto.Id = Guid.Parse(grpc.Id);
        dto.LeagueId = Guid.Parse(grpc.LeagueId);
        dto.SeasonYear = grpc.SeasonYear;
        dto.Round = grpc.Round;

        return dto;
    }
}
