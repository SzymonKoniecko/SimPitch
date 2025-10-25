using System.Security.Cryptography;
using EngineService.Application.DTOs;
using EngineService.Application.Features.IterationResults.Queries.GetIterationResultById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IterationResultController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IterationResultController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{iterationId}")]
        public async Task<ActionResult<IterationResultDto>> GetById([FromRoute] Guid iterationId)
        {
            var query = new GetIterationResultByIdQuery(iterationId);

            var response = await _mediator.Send(query);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
