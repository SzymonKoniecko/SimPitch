using EngineService.Application.DTOs;
using EngineService.Application.Features.Simulations.Commands.CreateSimulation;
using EngineService.Application.Features.Simulations.Queries.GetAllSimulations;
using EngineService.Application.Features.Simulations.Queries.GetSimulationById;
using EngineService.Infrastructure.Middlewares;
using MediatR;
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

        [HttpPost]
        public async Task<ActionResult<string>> CreateSimulationAsync([FromBody] SimulationParamsDto simulationParamsDto)
        {
            if (simulationParamsDto is null)
                throw new ValidationException("Simulation parameters cannot be null or empty.");

            var result = await mediator.Send(new CreateSimulationCommand(simulationParamsDto));
            if (result is null)
                throw new NotFoundException("Simulation result could not be generated.");

            return Ok(result);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<SimulationOverviewDto>>> GetAllAsync()
        {
            var result = await mediator.Send(new GetAllSimulationsQuery());
            if (result is null)
                throw new NotFoundException("No simulations or something went wrong");
            return Ok(result);
        }
        
        [HttpGet("{simulationId}")]
        public async Task<ActionResult<SimulationOverviewDto>> GetByIdAsync([FromRoute] Guid simulationId)
        {
            var result = await mediator.Send(new GetSimulationByIdQuery(simulationId));
            if (result is null)
                throw new NotFoundException("No simulations for given Id");
            return Ok(result);
        }
    }
}
