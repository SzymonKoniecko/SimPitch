using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.League.Queries.GetAllLeagues;
using SportsDataService.Infrastructure.Middlewares;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeagueController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<LeagueDto>>> GetLeaguesAsync()
        {
            var result = await _mediator.Send(new GetAllLeaguesQuery());

            if (result is null)
                throw new NotFoundException("No leagues.");

            return Ok(result);
        }
    }
}
