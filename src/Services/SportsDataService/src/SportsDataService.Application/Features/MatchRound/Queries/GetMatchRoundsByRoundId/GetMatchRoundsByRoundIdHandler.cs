using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;

public class GetMatchRoundsByRoundIdHandler : IRequestHandler<GetMatchRoundsByRoundIdQuery, IEnumerable<MatchRoundDto>>
{
    private readonly IMatchRoundReadRepository _MatchRoundReadRepository;
    private readonly IRedisRegistry _redisRegistry;

    public GetMatchRoundsByRoundIdHandler(
        IMatchRoundReadRepository MatchRoundReadRepository,
        IRedisRegistry redisRegistry)
    {
        _MatchRoundReadRepository = MatchRoundReadRepository;
        _redisRegistry = redisRegistry;
    }
    
    public async Task<IEnumerable<MatchRoundDto>> Handle(GetMatchRoundsByRoundIdQuery request, CancellationToken cancellationToken)
    {
        List<MatchRoundDto> requestedData = new();

        var cached = await _redisRegistry.GetMatchRoundsAsync(cancellationToken);
        if (cached == null || cached.Count() == 0)
        {
            var repoData = await _MatchRoundReadRepository.GetMatchRoundsAsync(cancellationToken);
            await _redisRegistry.SetMatchRoundsAsync(repoData, cancellationToken);
            requestedData = MatchRoundMapper.ListToDtos(repoData.Where(x => x.RoundId == request.roundId)).ToList();
        }
        else
        {
            requestedData = MatchRoundMapper.ListToDtos(cached.Where(x => x.RoundId == request.roundId)).ToList();
        }

        return requestedData;
    } 
}
