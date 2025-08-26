using System;
using MediatR;
using SimulationService.Application.Features.SimulationResults.DTOs;

namespace SimulationService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

public record GetSimulationResultsBySimulationIdQuery(Guid SimulationId) : IRequest<List<SimulationResultDto>>;