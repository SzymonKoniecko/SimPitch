using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.API.Mappers;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.LeagueRound.DTOs;
using SportsDataService.Application.Features.LeagueRound.Queries.GetAllLeagueRoundsByParams;
using SportsDataService.Domain.Enums;
using SportsDataService.Infrastructure.Middlewares;

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
        [Route("seasonYears")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetSeasonYearsAsync()
        {
            List<string> seasonYearsList = new();
            foreach (SeasonEnum season in Enum.GetValues(typeof(SeasonEnum)))
            {
                seasonYearsList.Add(EnumMapper.SeasonEnumToString(season));
            }

            return Ok(seasonYearsList);
        }
        
        [Route("seasons/{seasonYear}/leagues/{leagueId}/rounds")]
        [HttpGet]
        public async Task<ActionResult<List<LeagueRoundDto>>> GetLeagueRoundsAsync(
            [FromRoute] string seasonYear,
            [FromRoute] Guid leagueId,
            [FromQuery] Guid? leagueRoundId = default)
        {
            LeagueRoundFilterDto leagueRoundFilterDto = new();
            leagueRoundFilterDto.SeasonYear = seasonYear.Replace('_', '/');
            leagueRoundFilterDto.LeagueId = leagueId;

            if (leagueRoundId.HasValue && leagueRoundId.Value != Guid.Empty)
                leagueRoundFilterDto.LeagueRoundId = leagueRoundId.Value;


            var result = await _mediator.Send(new GetAllLeagueRoundsByParamsQuery(leagueRoundFilterDto));

            if (result is null)
                throw new NotFoundException("No league rounds for given params.");

            return Ok(result);
        }
    }
}
