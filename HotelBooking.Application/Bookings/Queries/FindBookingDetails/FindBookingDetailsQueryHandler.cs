using FluentResults;
using HotelBooking.Application.Interfaces;
using Mapster;
using MediatR;

namespace HotelBooking.Application.Bookings.Queries.FindBookingDetails;

public class FindBookingDetailsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindBookingDetailsQuery, Result<BookingDetailsDto>>
    {
    // Depends only on IUnitOfWork abstraction.
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<BookingDetailsDto>> Handle(FindBookingDetailsQuery request, CancellationToken cancellationToken)
        {
        // Async Call uses the NEW repository method.
        // The Application layer doesn't know how the data was fetched (EF Core Includes).
        var booking = await _unitOfWork.Bookings.GetDetailsByBookingNumberAsync(request.BookingNumber);

        // Error Handling
        if (booking == null)
            {
            return Result.Fail($"Booking reference '{request.BookingNumber}' not found.");
            }

        // Mapping
        var bookingDto = booking.Adapt<BookingDetailsDto>();

        return Result.Ok(bookingDto);
        }
    }