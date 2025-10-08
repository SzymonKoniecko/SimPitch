
using System;

namespace EngineService.Application.DTOs;

public class IterationPreviewDto
{
    public Guid ScoreboardId { get; set; }
    public int IterationIndex { get; set; }
    public Guid WinnerTeamId { get; set; }
    public int Points { get; set; }
    public string Raport { get; set; }
}
