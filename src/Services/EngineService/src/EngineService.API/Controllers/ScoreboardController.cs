using EngineService.Application.DTOs;
using EngineService.Application.Features.Scoreboards.Queries;
using EngineService.Application.Features.Scoreboards.Queries.GetScoreboardByLeagueIdAndSeasonYear;
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
        public async Task<ActionResult<List<ScoreboardDto>>> GetByIds(
            [FromQuery] Guid simulationId,
            [FromQuery] Guid iterationResultId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetScoreboardsBySimulationIdQuery(simulationId, iterationResultId, withTeamStats: true);

            var response = await mediator.Send(query, cancellationToken);
            if (response == null) return NotFound();
            return Ok(response);
        }
        

        [Route("seasons/{seasonYear}/leagues/{leagueId}/scoreboard")]
        [HttpGet]
        public async Task<ActionResult<List<ScoreboardDto>>> GetScoreboardByLeagueIdAndSeasonYear(
            [FromRoute] string seasonYear,
            [FromRoute] Guid leagueId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetScoreboardByLeagueIdAndSeasonYearQuery(leagueId, seasonYear.Replace('_', '/'));

            var response = await mediator.Send(query, cancellationToken);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
