using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Interfaces;

public interface IScoreboardWriteRepository
{
    Task CreateScoreboardAsync(Scoreboard scoreboard, CancellationToken cancellationToken);
}
