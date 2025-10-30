using FluentResults;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.FindBookingDetails;

/// <summary>
/// Query to find booking details based on the unique booking reference number.
/// </summary>
public record FindBookingDetailsQuery(string BookingNumber) : IRequest<Result<BookingDetailsDto>>;