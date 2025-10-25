using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.IterationResult;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using SimulationService.API.Mappers;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;
using System.ComponentModel.DataAnnotations;

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

    public override async Task<IterationResultByIdResponse> GetIterationResultById(IterationResultByIdRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            throw new ValidationException("Missing ID for iterationResult [gRPC]");
        }
        var query = new GetIterationResultByIdQuery(Guid.Parse(request.Id));

        var dto = await _mediator.Send(query, cancellationToken: context.CancellationToken);
        return new IterationResultByIdResponse
        {
            IterationResult = IterationResultMapper.ToProto(dto)
        };
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
