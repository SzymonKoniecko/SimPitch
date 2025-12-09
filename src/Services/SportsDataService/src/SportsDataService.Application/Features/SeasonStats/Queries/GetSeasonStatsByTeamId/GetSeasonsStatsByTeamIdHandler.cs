using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;

public class GetSeasonsStatsByTeamIdHandler : IRequestHandler<GetSeasonsStatsByTeamIdQuery, IEnumerable<SeasonStatsDto>>
{
    private readonly ISeasonStatsReadRepository _SeasonStatsRepository;

    public GetSeasonsStatsByTeamIdHandler(ISeasonStatsReadRepository SeasonStatsRepository)
    {
        _SeasonStatsRepository = SeasonStatsRepository;
    }

    public async Task<IEnumerable<SeasonStatsDto>> Handle(GetSeasonsStatsByTeamIdQuery request, CancellationToken cancellationToken)
    {
        var stats = await _SeasonStatsRepository.GetSeasonsStatsByTeamIdAsync(request.teamId, cancellationToken);
        if (stats is null)
        {
            return Enumerable.Empty<SeasonStatsDto>();
        }
        return stats.Select(x => SeasonStatsMapper.ToDto(x));
    }
}
