using EngineService.Application.DTOs;
using EngineService.Application.Features.Simulations.Queries.GetSimulationById;
using EngineService.Application.Features.SimulationStats.Queries.GetSimulationStatsBySimulationId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationStatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SimulationStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{simulationId}")]
        public async Task<ActionResult<List<SimulationTeamStatsDto>>> GetById(
            [FromRoute] Guid simulationId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetSimulationStatsBySimulationIdQuery(simulationId);

            var response = await _mediator.Send(query, cancellationToken);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
