using System;
using MediatR;
using SimulationService.Domain.ValueObjects;

namespace SimulationService.Application.Features.SeasonsStats.Queries.GetSeasonsStatsByTeamIdGrpc;

public record  GetSeasonsStatsByTeamIdGrpcQuery(Guid teamId) : IRequest<IEnumerable<SeasonStats>>;
