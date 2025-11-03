using System;
using MediatR;

namespace SimulationService.Application.Features.Scoreboards.Commands;

public class CreateScoreboardByIterationResultCommandHandler : IRequestHandler<CreateScoreboardByIterationResultCommand, bool>
{
    public CreateScoreboardByIterationResultCommandHandler()
    {

    }
    
    public async Task<bool> Handle(CreateScoreboardByIterationResultCommand command, CancellationToken cancellationToken)
    {
        return true;
    }
}
