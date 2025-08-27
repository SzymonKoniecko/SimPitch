using System;
using MediatR;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.Scoreboards.Commands.CreateScoreboard;

public class CreateScoreboardCommandHandler : IRequestHandler<CreateScoreboardCommand, Guid>
{
    private readonly IScoreboardWriteRepository _scoreboardWriteRepository;

    public CreateScoreboardCommandHandler(IScoreboardWriteRepository repository)
    {
        _scoreboardWriteRepository = repository;
    }

    public async Task<Guid> Handle(CreateScoreboardCommand request, CancellationToken cancellationToken)
    {
        var entity = 
        await _scoreboardWriteRepository.CreateScoreboardAsync(scoreboard, cancellationToken: cancellationToken);

        return scoreboard.Id;
    }
}
