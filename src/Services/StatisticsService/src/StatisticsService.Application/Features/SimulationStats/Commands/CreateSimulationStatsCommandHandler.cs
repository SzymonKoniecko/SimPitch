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

    public CreateSimulationStatsCommandHandler
    (
        IScoreboardTeamStatsReadRepository scoreboardTeamStatsReadRepository,
        ISimulationStatsService simulationStatsService,
        ISimulationTeamStatsWriteRepository simulationTeamStatsWriteRepository,
        ISimulationTeamStatsReadRepository simulationTeamStatsReadRepository
    )
    {
        _scoreboardTeamStatsReadRepository = scoreboardTeamStatsReadRepository;
        _simulationStatsService = simulationStatsService;
        _simulationTeamStatsWriteRepository = simulationTeamStatsWriteRepository;
        _simulationTeamStatsReadRepository = simulationTeamStatsReadRepository;
    }
    
    public async Task<(bool, Guid)> Handle(CreateSimulationStatsCommand command, CancellationToken cancellationToken)
    {
        if (await _simulationTeamStatsReadRepository.HasExactNumberOfSimulationTeamStatsAsync(command.SimulationId, 0, cancellationToken) == false)
        {
            return (true, command.SimulationId); // simulationTeamStats are already created
        }
        
        var scoreboardTeamStats = await _scoreboardTeamStatsReadRepository.GetScoreboardByScoreboardIdAsync(command.SimulationId, cancellationToken);

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
