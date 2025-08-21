using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;

public class GetMatchRoundsByRoundIdHandler : IRequestHandler<GetMatchRoundsByRoundIdQuery, IEnumerable<MatchRoundDto>>
{
    private readonly IMatchRoundReadRepository _MatchRoundReadRepository;
    public GetMatchRoundsByRoundIdHandler(IMatchRoundReadRepository MatchRoundReadRepository)
    {
        _MatchRoundReadRepository = MatchRoundReadRepository;
    }
    public async Task<IEnumerable<MatchRoundDto>> Handle(GetMatchRoundsByRoundIdQuery request, CancellationToken cancellationToken)
    {
        return MatchRoundMapper.ListToDtos(await _MatchRoundReadRepository.GetMatchRoundsByRoundIdAsync(request.roundId, cancellationToken));
    } 
}
