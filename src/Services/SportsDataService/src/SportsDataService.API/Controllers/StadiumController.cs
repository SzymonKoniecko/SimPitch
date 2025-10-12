using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.Stadium.Queries.GetAllStadiums;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StadiumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StadiumDto>>> GetStadiumsAsync()
        {
            var result = await _mediator.Send(new GetAllStadiumsQuery());

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
