using System;
using MediatR;
using SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationIdsByDate;

public class GetSimulationIdsByDateQueryHandler : IRequestHandler<GetSimulationIdsByDateQuery, List<Guid>>
{
    private readonly IMediator _mediatior;
    private readonly ISimulationOverviewReadRepository _simulationOverviewReadRepository;

    public GetSimulationIdsByDateQueryHandler(
        IMediator mediatior, 
        ISimulationOverviewReadRepository simulationOverviewReadRepository)
    {
        _mediatior = mediatior;
        _simulationOverviewReadRepository = simulationOverviewReadRepository;
    }
    public async Task<List<Guid>> Handle(GetSimulationIdsByDateQuery query, CancellationToken cancellationToken)
    {
        List<Guid> completedSimulations = new();
        
        var result = await _simulationOverviewReadRepository.GetSimulationIdsByDateAsync(query.requestedDate, cancellationToken);
        foreach (Guid simOverviewId in result)
        {
            var simState = await _mediatior.Send(new GetSimulationStateBySimulationIdQuery(simOverviewId), cancellationToken);
            if(simState.State.Equals("Completed"))
                completedSimulations.Add(simOverviewId);
        }
        
        return completedSimulations;
    }
}
