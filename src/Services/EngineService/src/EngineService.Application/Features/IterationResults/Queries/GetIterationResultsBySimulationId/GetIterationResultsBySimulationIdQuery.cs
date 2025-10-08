using System;
using MediatR;
using EngineService.Application.DTOs;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;

public record GetIterationResultsBySimulationIdQuery(Guid simulationId) : IRequest<List<IterationResultDto>>;