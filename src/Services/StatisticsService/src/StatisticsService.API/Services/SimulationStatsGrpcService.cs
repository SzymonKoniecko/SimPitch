using Grpc.Core;
using MediatR;
using SimPitchProtos.StatisticsService;
using SimPitchProtos.StatisticsService.SimulationStats;
using StatisticsService.Application.DTOs;

namespace StatisticsService.API.Services;

public class SimulationStatsGrpcService : SimulationStatsService.SimulationStatsServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SimulationStatsGrpcService> _logger;

    public SimulationStatsGrpcService(IMediator mediatr, ILogger<SimulationStatsGrpcService> logger)
    {
        _mediator = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<CreateSimulationStatsResponse> CreateSimulationStats(CreateSimulationStatsRequest request, ServerCallContext context)
    {
        //var command = new CreateScoreboardCommand(Guid.Parse(request.SimulationId));

        //var results = await _mediator.Send(command, cancellationToken: context.CancellationToken);

        return new CreateSimulationStatsResponse
        {
            IsCreated = true
        };
    }

    public override async Task<GetSimulationStatsResponse> GetSimulationStats(GetSimulationStatsRequest request, ServerCallContext context)
    {

        return new GetSimulationStatsResponse
        {
            SimulationTeamStats = { }
        };
    }

    public static SimulationTeamStatsGrpc SimulationStatsToGrpc(SimulationTeamStatsDto dto)
    {
        var grpc = new SimulationTeamStatsGrpc();

        grpc.Id = dto.Id.ToString();
        grpc.SimulationId = dto.SimulationId.ToString();
        grpc.TeamId = dto.TeamId.ToString();

        if (dto.PositionProbbility != null)
            grpc.PositionProbbility.AddRange(dto.PositionProbbility);

        grpc.AverangePoints = dto.AverangePoints;
        grpc.AverangeWins = dto.AverangeWins;
        grpc.AverangeLosses = dto.AverangeLosses;
        grpc.AverangeDraws = dto.AverangeDraws;
        grpc.AverangeGoalsFor = dto.AverangeGoalsFor;
        grpc.AverangeGoalsAgainst = dto.AverangeGoalsAgainst;

        return grpc;
    }
}
