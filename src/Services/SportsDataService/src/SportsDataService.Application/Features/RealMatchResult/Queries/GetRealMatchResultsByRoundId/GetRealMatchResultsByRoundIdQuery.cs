using System;
using MediatR;
using SportsDataService.Application.DTOs;

namespace SportsDataService.Application.Features.RealMatchResult.Queries.GetRealMatchResultsByRoundId;

public record GetRealMatchResultsByRoundIdQuery(Guid roundId) : IRequest<IEnumerable<RealMatchResultDto>>;