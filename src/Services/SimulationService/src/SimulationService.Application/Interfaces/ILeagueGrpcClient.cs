using System;
using SimulationService.Application.Features.Leagues.DTOs;

namespace SimulationService.Application.Interfaces;

public interface ILeagueGrpcClient
{
    Task<LeagueDto> GetLeagueByIdAsync(Guid leagueId);
}
