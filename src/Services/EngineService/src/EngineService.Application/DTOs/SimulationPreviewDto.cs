
using System;

namespace EngineService.Application.DTOs;

public class SimulationPreviewDto
{
    public Guid SimulationId { get; set; }
    public ScoreboardPreviewDto ScoreboardPreviewDto { get; set; }
}
