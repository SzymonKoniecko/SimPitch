using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.LeagueRound.DTOs;
using SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueRoundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeagueRoundController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("api/seasons/{seasonYear}/leagues/{leagueId}/rounds")]
        [HttpGet]
        public async Task<ActionResult<List<LeagueRoundDto>>> GetLeagueRoundsAsync(
            [FromRoute] string seasonYear,
            [FromRoute] Guid leagueId,
            [FromQuery] Guid? leagueRoundId = default)
        {
            LeagueRoundFilterDto leagueRoundFilterDto = new();
            leagueRoundFilterDto.SeasonYear = seasonYear;
            leagueRoundFilterDto.LeagueId = leagueId;

            if (leagueRoundId.HasValue && leagueRoundId.Value != Guid.Empty)
                leagueRoundFilterDto.LeagueRoundId = leagueRoundId.Value;


            var result = await _mediator.Send(new GetAllLeagueRoundsByParamsQuery(leagueRoundFilterDto));

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
