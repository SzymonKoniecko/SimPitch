using System;
using MediatR;
using SimulationService.Application.Features.Simulations.DTOs;
using SimulationService.Domain.Entities;
using SimulationService.Domain.Enums;
using SimulationService.Domain.Interfaces.Read;

namespace SimulationService.Application.Features.Simulations.Queries.GetSimulationStateBySimulationId;

public class GetSimulationStateBySimulationIdQueryHandler : IRequestHandler<GetSimulationStateBySimulationIdQuery, SimulationStateDto>
{
    private readonly ISimulationStateReadRepository _simulationStateReadRepository;

    public GetSimulationStateBySimulationIdQueryHandler(ISimulationStateReadRepository simulationStateReadRepository)
    {
        _simulationStateReadRepository = simulationStateReadRepository;
    }

    public async Task<SimulationStateDto> Handle(GetSimulationStateBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _simulationStateReadRepository.GetSimulationStateBySimulationIdAsync(query.simulationId, cancellationToken);
        return ToDto(result);
    }

    private SimulationStateDto ToDto(SimulationState result)
    {
        var dto = new SimulationStateDto();

        dto.Id = result.Id;
        dto.SimulationId = result.SimulationId;
        dto.LastCompletedIteration = result.LastCompletedIteration;
        dto.ProgressPercent = result.ProgressPercent;
        dto.UpdatedAt = result.UpdatedAt;
        
        switch (result.State)
        {
            case SimulationStatus.Pending:
                dto.State = "Pending";
                break;
            case SimulationStatus.Running:
                dto.State = "Running";
                break;
            case SimulationStatus.Completed:
                dto.State = "Completed";
                break;
            case SimulationStatus.Cancelled:
                dto.State = "Cancelled";
                break;
            case Domain.Enums.SimulationStatus.Failed:
                dto.State = "Failed";
                break;
            default:
                throw new KeyNotFoundException("Missing simulation state value");
        }
        return dto;
    }
}
