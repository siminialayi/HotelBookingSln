using HotelBooking.Application.Bookings.Commands.BookRoom;
using HotelBooking.Application.Bookings.Queries.FindBookingDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IMediator mediator) : ControllerBase
    {
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Books an available room between two dates for a specified number of guests.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BookRoom([FromBody] BookRoomRequest request)
        {
        var command = new BookRoomCommand
            {
            RoomId = request.RoomId,
            CheckInDate = request.CheckInDate.Date,
            CheckOutDate = request.CheckOutDate.Date,
            NumberOfGuests = request.NumberOfGuests,
            GuestName = request.GuestName
            };

        var result = await _mediator.Send(command);

        if (result.IsFailed)
            {
            // Mapping FluentResult to ProblemDetails
            return BadRequest(new ProblemDetails
                {
                Status = StatusCodes.Status400BadRequest,
                Title = "Booking failed due to business rule violation.",
                Detail = string.Join("; ", result.Errors.Select(e => e.Message)),
                Extensions = { { "errors", result.Errors.Select(e => new { e.Message, e.Metadata }) } }
                });
            }

        // Return 201 Created
        return CreatedAtAction(
            nameof(GetBooking),
            new { bookingNumber = result.Value },
            result.Value);
        }

    /// <summary>
    /// Retrieves the details of a specific booking using its unique reference number.
    /// </summary>
    [HttpGet("{bookingNumber}")]
    [ProducesResponseType(typeof(BookingDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking(string bookingNumber)
        {
        var query = new FindBookingDetailsQuery(bookingNumber);
        var result = await _mediator.Send(query);

        if (result.IsFailed)
            {
            return NotFound(result.Errors.First().Message);
            }

        return Ok(result.Value);
        }
    }