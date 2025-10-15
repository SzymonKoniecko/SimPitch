using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.Teams.Queries.GetAllTeams;
using SportsDataService.Infrastructure.Middlewares;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<TeamDto>>> GetTeamsAsync()
        {
            var result = await _mediator.Send(new GetAllTeamsQuery());

            if (result is null)
                throw new NotFoundException("No teams.");

            return Ok(result);
        }
    }
}
