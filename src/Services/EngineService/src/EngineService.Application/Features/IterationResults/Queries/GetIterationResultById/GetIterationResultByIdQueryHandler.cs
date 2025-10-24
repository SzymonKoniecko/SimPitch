using System;
using EngineService.Application.DTOs;
using EngineService.Application.Interfaces;
using MediatR;

namespace EngineService.Application.Features.IterationResults.Queries.GetIterationResultById;

public class GetIterationResultByIdQueryHandler : IRequestHandler<GetIterationResultByIdQuery, IterationResultDto>
{
    private readonly IIterationResultGrpcClient _client;

    public GetIterationResultByIdQueryHandler(IIterationResultGrpcClient client)
    {
        _client = client;
    }

    public async Task<IterationResultDto> Handle(GetIterationResultByIdQuery query, CancellationToken cancellationToken)
    {
        return await _client.GetIterationResultByIdAsync(query.iterationId, cancellationToken: cancellationToken);
    }
}
