using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.SeasonStats.Queries.GetSeasonStatsById;

public class GetSeasonStatsByIdHandler : IRequestHandler<GetSeasonStatsByIdQuery, SeasonStatsDto>
{
    private readonly ISeasonStatsReadRepository _SeasonStatsRepository;

    public GetSeasonStatsByIdHandler(ISeasonStatsReadRepository SeasonStatsRepository)
    {
        _SeasonStatsRepository = SeasonStatsRepository;
    }

    public async Task<SeasonStatsDto> Handle(GetSeasonStatsByIdQuery request, CancellationToken cancellationToken)
    {
        var stats = await _SeasonStatsRepository.GetSeasonStatsByIdAsync(request.seasonStatsId, cancellationToken);
        if (stats is null)
        {
            throw new KeyNotFoundException($" season stats with Id '{request.seasonStatsId}' was not found.");
        }
        return SeasonStatsMapper.ToDto(stats);
    }
}
