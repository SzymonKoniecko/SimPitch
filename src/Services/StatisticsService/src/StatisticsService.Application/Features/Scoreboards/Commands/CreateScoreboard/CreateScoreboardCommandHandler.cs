using System;
using MediatR;
using StatisticsService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public class CreateScoreboardCommandHandler : IRequestHandler<CreateScoreboardCommand, Guid>
{
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;
    private readonly IMediator _mediator;

    public CreateScoreboardCommandHandler(IScoreboardWriteRepository repository, IMediator mediator)
    {
        _scoreboardWriteRepository = repository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var simulationResultsQuery = new GetSimulationResultsBySimulationIdQuery(request.simulationimulationResultDto.SimulationId);
        var simulationResults = await _mediator.Send(simulationResultsQuery, cancellationToken);

        

        // await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, cancellationToken: cancellationToken);

        // return scoreboard.Id;
        return Guid.NewGuid();
    }
}
