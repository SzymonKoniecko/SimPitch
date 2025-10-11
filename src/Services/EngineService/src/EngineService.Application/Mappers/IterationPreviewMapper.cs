using System;
using EngineService.Application.DTOs;

namespace EngineService.Application.Mappers;

public static class IterationPreviewMapper
{

    public static List<IterationPreviewDto> GetIterationPreviewDtosAsync(List<ScoreboardTeamStatsDto> scoreboardTeamStatsDtos, IterationResultDto iterationResultDto)
    {
        if (iterationResultDto == null)
        {
            return new List<IterationPreviewDto>();
        }


        List<IterationPreviewDto> iterationPreviewDtos = new();

        foreach (var teamStatsInScoreboard in scoreboardTeamStatsDtos.OrderBy(x => x.Rank))
        {
            if (teamStatsInScoreboard.Rank == 1)
            {
                iterationPreviewDtos.Add(MapToIterationPreview(teamStatsInScoreboard, iterationResultDto));
            }
            else if (teamStatsInScoreboard.Rank == 2)
            {
                iterationPreviewDtos.Add(MapToIterationPreview(teamStatsInScoreboard, iterationResultDto));
            }
            else if (teamStatsInScoreboard.Rank == 3)
            {
                iterationPreviewDtos.Add(MapToIterationPreview(teamStatsInScoreboard, iterationResultDto));
            }
        }
        return iterationPreviewDtos;
    }

    public static IterationPreviewDto MapToIterationPreview(
        ScoreboardTeamStatsDto scoreboardTeamStatsDto,
        IterationResultDto iterationResultDto)
    {
        var dto = new IterationPreviewDto();

        dto.ScoreboardId = scoreboardTeamStatsDto.ScoreboardId;
        dto.IterationIndex = iterationResultDto.IterationIndex;
        dto.TeamId = scoreboardTeamStatsDto.TeamId;
        dto.Points = scoreboardTeamStatsDto.Points;
        dto.Rank = scoreboardTeamStatsDto.Rank;

        return dto;
    }
}
