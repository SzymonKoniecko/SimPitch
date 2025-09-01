using System;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Domain.Services;

public class ScoreboardService
{

    public ScoreboardService()
    {
    }

    public Scoreboard CalcucalteScoreboard(Guid simulationId)
    { 
        return new Scoreboard();
    }
}
