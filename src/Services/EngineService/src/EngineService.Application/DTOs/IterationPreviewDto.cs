
using System;

namespace EngineService.Application.DTOs;

public class IterationPreviewDto
{
    public Guid IterationId { get; set; }
    public Guid ScoreboardId { get; set; }
    public int IterationIndex { get; set; }
    public Guid TeamId { get; set; }
    public int Points { get; set; }
    public int Rank { get; set; }
}
