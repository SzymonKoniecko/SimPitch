using System;
using MediatR;
using SimulationService.Application.Features.LeagueRounds.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.LeagueRounds.Queries.GetLeagueRoundsByParamsGrpc;

public record GetLeagueRoundsByParamsGrpcQuery(LeagueRoundDtoRequest leagueRoundDtoRequest) : IRequest<List<LeagueRound>>;