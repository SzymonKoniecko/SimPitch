using EngineService.Application.DTOs;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardsBySimulationId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreboardController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScoreboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        public async Task<List<ScoreboardDto>> GetScoreboardDtosAsync([FromQuery] Guid simulationId, [FromQuery] Guid iterationResultId)
        {
            var query = new GetScoreboardsBySimulationIdQuery(simulationId, iterationResultId, withTeamStats: true);

            return await mediator.Send(query);
        }
    }
}
