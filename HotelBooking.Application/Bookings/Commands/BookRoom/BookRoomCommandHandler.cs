using FluentResults;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using MediatR;

namespace HotelBooking.Application.Bookings.Commands.BookRoom;

public class BookRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BookRoomCommand, Result<string>>
    {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<string>> Handle(BookRoomCommand request, CancellationToken cancellationToken)
        {

        // Business Rule Enforcement: Capacity Check 

        var roomCapacity = await _unitOfWork.Rooms.GetRoomCapacityAsync(request.RoomId);

        if (roomCapacity == 0)
            {
            return Result.Fail($"Room with ID {request.RoomId} was not found.");
            }

        // Business Rule: A room cannot be occupied by more people than its capacity.
        if (request.NumberOfGuests > roomCapacity)
            {
            return Result.Fail(new Error("Capacity exceeded.")
                .WithMetadata("MaxCapacity", roomCapacity)
                .WithMetadata("RequestedGuests", request.NumberOfGuests)
                .WithMetadata("Rule", "Capacity"));
            }

        // Business Rule: A room cannot be double booked for any given night.
        var isDoubleBooked = await _unitOfWork.Bookings.IsRoomDoubleBookedAsync(
            request.RoomId,
            request.CheckInDate,
            request.CheckOutDate);

        if (isDoubleBooked)
            {
            return Result.Fail(new Error("Room is already booked for the requested period (overlapping booking).")
                .WithMetadata("Rule", "DoubleBooking"));
            }

        // Business Rule Enforcement: Room Changes, Booking Creation
        // Enforced by design, as the transaction creates one booking for one room/period.)
        // Create and Save the Booking 
        // Business Rule: Booking Numbers must be unique. (Utilizing a GUID-based unique number)
        var bookingNumber = $"BK-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";

        var newBooking = new Booking
            {
            Id = Guid.NewGuid(),
            BookingNumber = bookingNumber,
            RoomId = request.RoomId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfGuests = request.NumberOfGuests
            };

        // Add the entity and commit the Unit of Work
        await _unitOfWork.Bookings.AddAsync(newBooking);
        await _unitOfWork.CompleteAsync(); // Saves all changes in this context

        // Return the unique Booking Number to the user for reference.
        return Result.Ok(bookingNumber);
        }
    }