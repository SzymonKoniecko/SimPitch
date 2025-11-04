using System;
using MediatR;
using SimulationService.Application.Interfaces;

namespace SimulationService.Application.Features.Scoreboards.Commands;

public class CreateScoreboardByIterationResultCommandHandler : IRequestHandler<CreateScoreboardByIterationResultCommand, bool>
{
    private readonly IScoreboardGrpcClient _scoreboardGrpcClient;

    public CreateScoreboardByIterationResultCommandHandler(IScoreboardGrpcClient scoreboardGrpcClient)
    {
        _scoreboardGrpcClient = scoreboardGrpcClient;
    }
    
    public async Task<bool> Handle(CreateScoreboardByIterationResultCommand command, CancellationToken cancellationToken)
    {
        return await _scoreboardGrpcClient.CreateScoreboardByIterationResultDataAsync(command.OverviewDto, command.IterationResultDto, cancellationToken: cancellationToken);
    }
}
