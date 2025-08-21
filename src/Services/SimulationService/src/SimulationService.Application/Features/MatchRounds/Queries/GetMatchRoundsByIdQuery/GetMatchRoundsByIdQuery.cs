using System;
using MediatR;
using SimulationService.Application.Features.MatchRounds.DTOs;
using SimulationService.Domain.Entities;

namespace SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;

public record GetMatchRoundsByIdQuery(Guid RoundId) : IRequest<List<MatchRound>>;