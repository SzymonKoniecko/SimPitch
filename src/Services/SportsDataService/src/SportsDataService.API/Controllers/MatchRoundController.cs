using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByParams;
using SportsDataService.Application.Features.MatchRound.Queries.GetMatchRoundsByRoundId;
using SportsDataService.Infrastructure.Middlewares;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchRoundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchRoundController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("seasons/{seasonYear}/leagues/{leagueId}/matchrounds")]
        [HttpGet]
        public async Task<ActionResult<List<MatchRoundDto>>> GetMatchRoundsAsync(
            [FromRoute] string seasonYear,
            [FromRoute] Guid leagueId,
            CancellationToken cancellationToken = default)
        {
            seasonYear = seasonYear.Replace('_', '/');
            var result = await _mediator.Send(new GetMatchRoundsByParamsQuery(leagueId, seasonYear), cancellationToken);

            if (result is null)
                throw new NotFoundException($"No match rounds by leagueId:{leagueId} and season Year: {seasonYear}");

            return Ok(result);
        }
    }
}
