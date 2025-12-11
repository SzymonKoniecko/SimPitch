using System;
using MediatR;
using StatisticsService.Application.Interfaces;
using StatisticsService.Domain.Entities;
using StatisticsService.Domain.Interfaces;

namespace StatisticsService.Application.Features.SimulationStats.Commands;

public class CreateSimulationStatsCommandHandler : IRequestHandler<CreateSimulationStatsCommand, (bool, Guid)>
{
    private readonly IScoreboardTeamStatsReadRepository _scoreboardTeamStatsReadRepository;
    private readonly ISimulationStatsService _simulationStatsService;
    private readonly ISimulationTeamStatsWriteRepository _simulationTeamStatsWriteRepository;
    private readonly ISimulationTeamStatsReadRepository _simulationTeamStatsReadRepository;
    private readonly IScoreboardReadRepository _scoreboardReadRepository;

    public CreateSimulationStatsCommandHandler
    (
        IScoreboardTeamStatsReadRepository scoreboardTeamStatsReadRepository,
        ISimulationStatsService simulationStatsService,
        ISimulationTeamStatsWriteRepository simulationTeamStatsWriteRepository,
        ISimulationTeamStatsReadRepository simulationTeamStatsReadRepository,
        IScoreboardReadRepository scoreboardReadRepository
    )
    {
        _scoreboardTeamStatsReadRepository = scoreboardTeamStatsReadRepository;
        _simulationStatsService = simulationStatsService;
        _simulationTeamStatsWriteRepository = simulationTeamStatsWriteRepository;
        _simulationTeamStatsReadRepository = simulationTeamStatsReadRepository;
        _scoreboardReadRepository = scoreboardReadRepository;
    }
    
    public async Task<(bool, Guid)> Handle(CreateSimulationStatsCommand command, CancellationToken cancellationToken)
    {
        if (await _simulationTeamStatsReadRepository.HasExactNumberOfSimulationTeamStatsAsync(command.SimulationId, 0, cancellationToken) == false)
        {
            return (true, command.SimulationId); // simulationTeamStats are already created
        }
        var scoreboards = await _scoreboardReadRepository.GetScoreboardByQueryAsync(command.SimulationId, iterationResultId: Guid.Empty, withTeamStats: false, cancellationToken);
        List<ScoreboardTeamStats> scoreboardTeamStats = new();

        foreach (var scoreboard in scoreboards)
        {
            var teamStats = await _scoreboardTeamStatsReadRepository.GetScoreboardByScoreboardIdAsync(scoreboard.Id, cancellationToken);
            scoreboardTeamStats.AddRange(teamStats.Where(x => x.IsInitialStat == false)); // only full teamStats are needed
        }

        List<SimulationTeamStats> simulationTeamStats = _simulationStatsService.CalculateSimulationStatsForTeams(scoreboardTeamStats.OrderBy(x => x.TeamId).ToList(), command.SimulationId);

        foreach (var item in simulationTeamStats)
        {
            await _simulationTeamStatsWriteRepository.CreateSimulationTeamStatsAsync(item, cancellationToken);
        }

        return(
            await _simulationTeamStatsReadRepository.HasExactNumberOfSimulationTeamStatsAsync(command.SimulationId, simulationTeamStats.Count, cancellationToken),
            command.SimulationId
        );
    }
}
