using System;
using MediatR;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationIdsByDate;

public class GetSimulationIdsByDateQueryHandler : IRequestHandler<GetSimulationIdsByDateQuery, List<string>>
{
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetSimulationIdsByDateQueryHandler(ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }
    public async Task<List<string>> Handle(GetSimulationIdsByDateQuery query, CancellationToken cancellationToken)
    {
        var result = await _simulationOverviewReadRepository.GetSimulationIdsByDateAsync(query.requestedDate, cancellationToken);
        
        return result.ToList();
    }
}
