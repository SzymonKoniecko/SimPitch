using EngineService.Application.DTOs;
using EngineService.Application.Features.Simulations.GetSimulationById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {
        private readonly IMediator mediator;

        public SimulationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{simulationId}")]
        public async Task<ActionResult<SimulationDto>> GetById([FromRoute] Guid simulationId)
        {
            var result = await mediator.Send(new GetSimulationByIdQuery(simulationId));
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
