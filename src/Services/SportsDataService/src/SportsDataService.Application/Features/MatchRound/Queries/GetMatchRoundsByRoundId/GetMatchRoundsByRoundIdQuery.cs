using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;

public record GetMatchRoundsByRoundIdQuery(Guid roundId) : IRequest<IEnumerable<MatchRoundDto>>;