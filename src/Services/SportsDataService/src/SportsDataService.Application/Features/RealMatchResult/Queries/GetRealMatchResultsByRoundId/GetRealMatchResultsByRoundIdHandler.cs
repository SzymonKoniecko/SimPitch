using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.RealMatchResult.Queries.GetRealMatchResultsByRoundId;

public class GetRealMatchResultsByRoundIdHandler : IRequestHandler<GetRealMatchResultsByRoundIdQuery, IEnumerable<RealMatchResultDto>>
{
    private readonly IRealMatchResultReadRepository _realMatchResultReadRepository;
    public GetRealMatchResultsByRoundIdHandler(IRealMatchResultReadRepository realMatchResultReadRepository)
    {
        _realMatchResultReadRepository = realMatchResultReadRepository;
    }
    public async Task<IEnumerable<RealMatchResultDto>> Handle(GetRealMatchResultsByRoundIdQuery request, CancellationToken cancellationToken)
    {
        return RealMatchResultMapper.ListToDtos(await _realMatchResultReadRepository.GetRealMatchResultsByRoundIdAsync(request.roundId, cancellationToken));
    } 
}
