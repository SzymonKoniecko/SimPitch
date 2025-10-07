using System;

namespace EngineService.Application.DTOs;

public class ScoreboardPreviewDto
{
    public Guid ScoreboardId { get; set; }
    public string WinnerId { get; set; }
    public string WinnerPoints { get; set; }
}
