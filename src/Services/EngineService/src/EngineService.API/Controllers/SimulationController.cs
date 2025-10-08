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

        [HttpGet]
        public async Task<SimulationDto> GetSimulationByIdAsync([FromRoute] Guid simulationId)
        {
            var query = new GetSimulationByIdQuery(simulationId);

            return await mediator.Send(query);
        }
    }
}
