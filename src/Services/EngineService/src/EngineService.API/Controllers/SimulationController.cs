using EngineService.Application.Common.Pagination;
using EngineService.Application.Common.Sorting;
using EngineService.Application.DTOs;
using EngineService.Application.Features.Simulations.Commands.CreateSimulation;
using EngineService.Application.Features.Simulations.Commands.StopSimulation;
using EngineService.Application.Features.Simulations.Queries.GetAllSimulationOverviews;
using EngineService.Application.Features.Simulations.Queries.GetSimulationById;
using EngineService.Application.Features.Simulations.Queries.GetSimulationOverviewBySimulationId;
using EngineService.Infrastructure.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
                return NotFound("Simulation result could not be generated.");

            return Ok(result);
        }
        
        [HttpGet("overview/{simulationId}")]
        public async Task<ActionResult<SimulationOverviewDto>> GetSimulationOverviewAsync(
            [FromRoute] Guid simulationId,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(
                new GetSimulationOverviewBySimulationIdQuery(simulationId), 
                cancellationToken);
            if (result is null)
                return NotFound("No simulation overview or something went wrong");
            return Ok(result);
        }

        [HttpGet("overviews")]
        public async Task<ActionResult<PagedResponse<SimulationOverviewDto>>> GetAllAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortingOption = "CreatedDate",
            [FromQuery] string order = "DESC",
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(
                new GetAllSimulationOverviewsQuery(
                    new PagedRequest((pageNumber - 1) * pageSize, pageSize, sortingOption, order)
                ),
                cancellationToken
            );
            if (result is null)
                return NotFound("No simulations or something went wrong");
            return Ok(result);
        }

        [HttpGet("{simulationId}")]
        public async Task<ActionResult<SimulationDto>> GetByIdAsync(
            [FromRoute] Guid simulationId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortingOption = "CreatedDate",
            [FromQuery] string order = "DESC",
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(
                new GetSimulationByIdQuery(simulationId,
                    new PagedRequest((pageNumber - 1) * pageSize, pageSize, sortingOption, order)
                ),
                cancellationToken
            );
            if (result == null)
                return NotFound("No simulations for given Id");
            return Ok(result);
        }

        [HttpDelete("stop/{simulationId}")]
        public async Task<ActionResult<SimulationDto>> StopSimulationBySimulationIdAsync(
            [FromRoute] Guid simulationId,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new StopSimulationCommand(simulationId), cancellationToken);
            if (result is null)
                return NotFound("Something went wrong with stop simulation workflow!");
            return Ok(result);
        }
    }
}
