using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.IterationResult;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using SimulationService.API.Mappers;

namespace SimulationService.API.Services;

public class IterationResultGrpcService : IterationResultService.IterationResultServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SimulationEngineGrpcService> _logger;

    public IterationResultGrpcService(IMediator mediator, ILogger<SimulationEngineGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<IterationResultsBySimulationIdResponse> GetIterationResultsBySimulationId(IterationResultsBySimulationIdRequest request, ServerCallContext context)
    {
        var query = new GetIterationResultsBySimulationIdQuery(Guid.Parse(request.SimulationId));

        List<IterationResultDto> IterationResults = await _mediator.Send(query, cancellationToken: context.CancellationToken);

        return new IterationResultsBySimulationIdResponse
        {
            IterationResults = { IterationResults.Select(sr => IterationResultMapper.ToProto(sr)) }
        };
    }
}
