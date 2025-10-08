using System;
using EngineService.Application.DTOs;
using EngineService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using EngineService.Application.Mappers;
using MediatR;

namespace EngineService.Application.Features.Simulations.GetSimulationById;

public class GetSimulationByIdQueryHandler : IRequestHandler<GetSimulationByIdQuery, SimulationDto>
{
    private readonly IMediator mediator;

    public GetSimulationByIdQueryHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    public async Task<SimulationDto> Handle(GetSimulationByIdQuery query, CancellationToken cancellationToken)
    {
        var iterationsQuery = new GetIterationResultsBySimulationIdQuery(query.simulationId);


        List<IterationResultDto> iterationResults = await mediator.Send(iterationsQuery, cancellationToken);
        
        if (iterationResults == null || iterationResults.Count == 0)
        {
            return null;
        }
        
        return SimulationMapper.ToSimulationDto(
                query.simulationId,
                iterationResults.Count,
                iterationResults.First().SimulationParams,
                new List<IterationPreviewDto>(),
                iterationResults.First()?.SimulatedMatchRounds.Count,
                (float)(iterationResults.First()?.PriorLeagueStrength)
            );
    }
}
