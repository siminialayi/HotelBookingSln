using HotelBooking.Application.Hotels.Queries.FindHotel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(IMediator mediator) : ControllerBase
    {
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Finds a hotel based on its name.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel([FromQuery] string name)
        {
        if (string.IsNullOrWhiteSpace(name))
            {
            return BadRequest("Hotel name is required for search.");
            }

        var query = new FindHotelQuery(name);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            {

            return NotFound(result.Errors[0].Message);
            }

        return Ok(result.Value);
        }
    }