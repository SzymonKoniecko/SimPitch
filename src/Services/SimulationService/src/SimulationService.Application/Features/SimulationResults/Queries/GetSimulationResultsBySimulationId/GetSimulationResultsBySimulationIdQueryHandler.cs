using System;
using MediatR;
using SimulationService.Application.Features.SimulationResults.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;

public class GetSimulationResultsBySimulationIdQueryHandler : IRequestHandler<GetSimulationResultsBySimulationIdQuery, List<SimulationResultDto>>
{
    private readonly ISimulationResultReadRepository _simulationResultReadRepository;

    public GetSimulationResultsBySimulationIdQueryHandler(ISimulationResultReadRepository simulationResultReadRepository)
    {
        _simulationResultReadRepository = simulationResultReadRepository;
    }

    public async Task<List<SimulationResultDto>> Handle(GetSimulationResultsBySimulationIdQuery request, CancellationToken cancellationToken)
    {
        var simulationResults = await _simulationResultReadRepository.GetSimulationResultsBySimulationIdAsync(request.SimulationId, cancellationToken);

        return simulationResults.Select(sr => SimulationResultMapper.ToDto(sr)).ToList();
    }
}
