using MediatR;
using EngineService.Application.DTOs;
using EngineService.Application.Mappers;
using EngineService.Application.Interfaces;

namespace EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;

public class GetScoreboardsBySimulationIdQueryHandler : IRequestHandler<GetScoreboardsBySimulationIdQuery, List<ScoreboardDto>>
{
    private readonly IScoreboardGrpcClient scoreboardGrpcClient;

    public GetScoreboardsBySimulationIdQueryHandler(IScoreboardGrpcClient scoreboardGrpcClient)
    {
        this.scoreboardGrpcClient = scoreboardGrpcClient;
    }

    public async Task<List<ScoreboardDto>> Handle(GetScoreboardsBySimulationIdQuery query, CancellationToken cancellationToken)
    {
        return await scoreboardGrpcClient.GetScoreboardsByQueryAsync(query.simulationId, cancellationToken, query.iterationId, query.withTeamStats);
    }
}
