using HotelBooking.Application.Rooms.Queries.FindAvailableRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IMediator mediator) : ControllerBase
    {
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Finds all available rooms for a given date range and guest count.
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IEnumerable<AvailableRoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] DateTime checkIn,
        [FromQuery] DateTime checkOut,
        [FromQuery] int guests)
        {
        var query = new FindAvailableRoomsQuery(checkIn.Date, checkOut.Date, guests);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            {
            // Handles basic validation errors (e.g., CheckOut before CheckIn)
            return BadRequest(new ProblemDetails
                {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid room search parameters.",
                Detail = string.Join("; ", result.Errors.Select(e => e.Message))
                });
            }

        return Ok(result.Value);
        }
    }