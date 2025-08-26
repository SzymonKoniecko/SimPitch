using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.SimulationResult;
using SimulationService.Application.Features.SimulationResults.DTOs;
using SimulationService.Application.Features.SimulationResults.Queries.GetSimulationResultsBySimulationId;
using SimulationService.API.Mappers;

namespace SimulationService.API.Services;

public class SimulationResultGrpcService : SimulationResultService.SimulationResultServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SimulationEngineGrpcService> _logger;

    public SimulationResultGrpcService(IMediator mediator, ILogger<SimulationEngineGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<SimulationResultsBySimulationIdResponse> GetSimulationResultsBySimulationId(SimulationResultsBySimulationIdRequest request, ServerCallContext context)
    {
        var query = new GetSimulationResultsBySimulationIdQuery(Guid.Parse(request.SimulationId));

        List<SimulationResultDto> simulationResults = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        return new SimulationResultsBySimulationIdResponse
        {
            SimulationResults = { simulationResults.Select(sr => SimulationResultMapper.ToProto(sr)) }
        };
    }
}
