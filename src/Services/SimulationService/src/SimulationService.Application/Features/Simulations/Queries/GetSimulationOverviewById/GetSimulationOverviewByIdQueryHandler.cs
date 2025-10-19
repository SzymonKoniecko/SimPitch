using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Application.Mappers;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationOverviewById;

public class GetSimulationOverviewByIdQueryHandler : IRequestHandler<GetSimulationOverviewByIdQuery, SimulationOverviewDto>
{
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetSimulationOverviewByIdQueryHandler(ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }

    public async Task<SimulationOverviewDto> Handle(GetSimulationOverviewByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _simulationOverviewReadRepository.GetSimulationOverviewByIdAsync(query.simulationId, cancellationToken);

        return SimulationOverviewMapper.ToDto(result);
    }
}
