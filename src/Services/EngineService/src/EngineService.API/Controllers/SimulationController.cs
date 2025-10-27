using EngineService.Application.Common.Pagination;
using EngineService.Application.DTOs;
using EngineService.Application.Features.Simulations.Commands.CreateSimulation;
using EngineService.Application.Features.Simulations.Commands.StopSimulation;
using EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;
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
        public async Task<ActionResult<string>> CreateSimulationAsync(
            [FromBody] SimulationParamsDto simulationParamsDto,
            CancellationToken cancellationToken = default)
        {
            if (simulationParamsDto is null)
                throw new ValidationException("Simulation parameters cannot be null or empty.");

            var result = await mediator.Send(new CreateSimulationCommand(simulationParamsDto), cancellationToken);
            if (result is null)
                throw new NotFoundException("Simulation result could not be generated.");

            return Ok(result);
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedResponse<SimulationOverviewDto>>> GetAllAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetAllSimulationOverviewsQuery(pageNumber, pageSize), cancellationToken);
            if (result is null)
                throw new NotFoundException("No simulations or something went wrong");
            return Ok(result);
        }

        [HttpGet("{simulationId}")]
        public async Task<ActionResult<SimulationDto>> GetByIdAsync(
            [FromRoute] Guid simulationId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new GetSimulationByIdQuery(simulationId, pageNumber, pageSize), cancellationToken);
            if (result is null)
                throw new NotFoundException("No simulations for given Id");
            return Ok(result);
        }
        
        [HttpDelete("stop/{simulationId}")]
        public async Task<ActionResult<SimulationDto>> StopSimulationBySimulationIdAsync(
            [FromRoute] Guid simulationId,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new StopSimulationCommand(simulationId), cancellationToken);
            if (result is null)
                throw new NotFoundException("Something went wrong with stop simulation workflow!");
            return Ok(result);
        }
    }
}
