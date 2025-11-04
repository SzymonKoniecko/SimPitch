using System;
using MediatR;

namespace StatisticsService.Application.Features.SimulationStats.Commands;

public class CreateSimulationStatsCommandHandler : IRequestHandler<CreateSimulationStatsCommand, bool>
{
    public CreateSimulationStatsCommandHandler()
    {

    }
    
    public async Task<bool> Handle(CreateSimulationStatsCommand command, CancellationToken cancellationToken)
    {
        return true;
    }
}
