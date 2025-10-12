using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsDataService.Application.DTOs;
using SportsDataService.Application.Features.Country.Queries.GetAllCountries;
using SportsDataService.Application.Features.Country.Queries.GetCountryById;

namespace SportsDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CountryDto>>> GetCountriesAsync()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
