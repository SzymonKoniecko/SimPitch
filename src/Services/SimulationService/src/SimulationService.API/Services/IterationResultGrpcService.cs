using Grpc.Core;
using MediatR;
using SimPitchProtos.SimulationService.IterationResult;
using SimulationService.Application.Features.IterationResults.DTOs;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultsBySimulationId;
using SimulationService.API.Mappers;
using SimulationService.Application.Features.IterationResults.Queries.GetIterationResultById;
using System.ComponentModel.DataAnnotations;
using Google.Protobuf;
using SimulationService.API.Helpers;
using SimulationService.Domain.ValueObjects;
using SimulationService.Application.Common.Pagination;
using SimulationService.Application.Mappers;
using IterationResultMapper = SimulationService.API.Mappers.IterationResultMapper;
using SimulationService.Domain.Consts;

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
    public override async Task GetIterationResultsBySimulationId(
        IterationResultsBySimulationIdRequest request,
        IServerStreamWriter<IterationResultsBySimulationIdResponse> responseStream,
        ServerCallContext context)
    {
        var simulationId = Guid.Parse(request.SimulationId);


        var query = new GetIterationResultsBySimulationIdQuery(
            simulationId,
            ProtoHelper.ValidateProtoAndMapToDto(request.PagedRequest)
        );

        var (results, details) = await _mediator.Send(query, cancellationToken: context.CancellationToken);
        
        await GrpcStreamHelper.StreamListAsync(
            results.Select(IterationResultMapper.ToProto),
            responseStream,
            items => new IterationResultsBySimulationIdResponse
            {
                Items = { items },
                Paged = ProtoHelper.MapPagedResultsToProto(details)
            },
            chunkSizeBytes: GrpcConsts.CHUNK_SIZE,
            context.CancellationToken
        );

        _logger.LogInformation($"Streamed {results.Count} iteration results for simulation {simulationId} (total={details.TotalCount})");
    }
}
