using System;
using MediatR;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Mappers;
using SportsDataService.Domain.Interfaces.Read;

namespace SportsDataService.Application.Features.FootballSeasonStats.Queries.GetFootballSeasonStatsById;

public class GetFootballSeasonStatsByIdHandler : IRequestHandler<GetFootballSeasonStatsByIdQuery, FootballSeasonStatsDto>
{
    private readonly IFootballSeasonStatsReadRepository _footballSeasonStatsRepository;

    public GetFootballSeasonStatsByIdHandler(IFootballSeasonStatsReadRepository footballSeasonStatsRepository)
    {
        _footballSeasonStatsRepository = footballSeasonStatsRepository;
    }

    public async Task<FootballSeasonStatsDto> Handle(GetFootballSeasonStatsByIdQuery request, CancellationToken cancellationToken)
    {
        var stats = await _footballSeasonStatsRepository.GetSeasonStatsByIdAsync(request.FootballSeasonStatsId, cancellationToken);
        if (stats is null)
        {
            throw new KeyNotFoundException($"Football season stats with Id '{request.FootballSeasonStatsId}' was not found.");
        }
        return FootballSeasonStatsMapper.ToDto(stats);
    }
}
