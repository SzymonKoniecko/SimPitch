using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;

namespace SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;

public record GetLeagueRoundsByParamsGrpcQuery(string seasonYear, int? round, Guid? leagueRoundId) : IRequest<List<LeagueRoundDto>>;