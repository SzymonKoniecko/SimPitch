using System;
using MediatR;
using SimulationService.Application.Features.MatchRounds.DTOs;

namespace SimulationService.Application.Features.MatchRounds.Queries.GetMatchRoundsByIdQuery;

public record GetMatchRoundsByIdQuery(Guid RoundId) : IRequest<List<MatchRoundDto>>;